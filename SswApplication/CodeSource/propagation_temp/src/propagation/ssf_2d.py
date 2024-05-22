# This file is part of SSW-2D.
# SSW-2D is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
# as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
#
# SSW-2D is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty
# of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
#
# You should have received a copy of the GNU General Public License along with Foobar. If not, see
# <https://www.gnu.org/licenses/>.

##
# @file ssf_2d.py
#
# @author R. Douvenot
# @package SSF_2D
# @date 16/02/23
# @version In progress
# @brief The core of the 2D SSF: propagates a 1D initial field with SSW in 2D.
# @details This function calculates the field using the classical SSF algorithm. It aims at giving comparisons for SSW
# and WWP. It propagates a 1D initial reduced field with SSF in 2D, in a Cartesian environment. The output field is
# given in the space domain. \n
# The refractivity is taken into account with a phase screen. Apodisation is also applied at each step.
#
# @param[in] u_0 Initial field. Complex array of size (N_z)
# @param[in] config Class that contains the configuration. See class list for details
# @param[in] config_source Class that contains the source configuration. See class list for details
# @param[in] n_refraction Vector containing the modified refractive index n. Real array of size (N_z)
# @param[in] z_relief Vector containing the relief indices at each distance step. Real array of size (N_z)
# @param[out] U_tot Wavelet coefficients of the 3D field. Complex array of size (N_x, N_z)
# @warning Put lengths = multiple of 2^l
##

# u_x_dx = SSW(u_0,simulation_parameters)
#
#######################################################################################################################

import copy
import math
import numpy as np
import time
import scipy.constants as cst
from src.propagation.apodisation import apply_apodisation, apodisation_window
from src.atmosphere.genere_n_profile import genere_phi_turbulent
from src.DSSF.dmft import dmft_parameters, u2w, w2u, surface_wave_propagation
from src.propagation.refraction import apply_refractive_index, apply_phi_turbulent
from src.DSSF.propa_discrete_spectral_domain import discrete_spectral_propagator, discrete_spectral_propagator_sin
from src.DSSF.dssf_one_step import dssf_one_step, dssf_one_step_cos, dssf_one_step_sin
import pywt
from src.wavelets.wavelet_operations import sparsify  # for sparsify
from src.propagation.ssw_2d import shift_relief

# put relief below the field (upward shift of the field)
def shift_relief_ssf(u_field, ii_relief):
    if ii_relief == 0:
        u_field_shifted = u_field
    else:
        u_field_shifted = np.zeros_like(u_field)
        u_field_shifted[ii_relief:] = u_field[:-ii_relief]
    return u_field_shifted

def ssf_2d(u_0, config, n_refraction, ii_vect_relief):

    # Simulation parameters
    n_x = config.N_x
    n_z = config.N_z
    # --- Creation of the apodisation window --- # @todo Code other apodisation windows
    # along z
    n_apo_z = int(config.apo_z * config.N_z)
    apo_window_z = apodisation_window(config.apo_window, n_apo_z)
    # ------------------------------------------ #
    # --- Initialisations --- #
    # initial field
    u_x = apply_apodisation(u_0, apo_window_z, config)
    if config.ground == 'Dielectric' or config.ground == 'PEC':
        # derivative of the relief calculated once for all
        diff_relief = np.diff(ii_vect_relief)
    # field after delta x
    u_x_dx = np.zeros_like(u_x)
    
    # ----------------------- #
    # saved total wavelet parameters (coo matrix, for storage only)
    wv_total = [[]] * n_x
    #e_total = [[]] * n_x
    e_total = np.zeros((n_x, n_z), dtype='complex')

    # --- propagator --- %
    if config.ground == 'None':
        propagator_dssf = discrete_spectral_propagator(config, config.N_z)
    elif config.ground == 'PEC':
        propagator_dssf = discrete_spectral_propagator_sin(config, config.N_z)

    # Loop over the x_axis
    for ii_x in np.arange(1, n_x+1):
        if ii_x % 100 == 0:
            print('Iteration', ii_x, '/', n_x, '. Distance =', ii_x*config.x_step)
        # --- apodisation --- #
        u_x = apply_apodisation(u_x, apo_window_z, config)
        # ------------------- #

        # --- refractivity applied twice 1/2 --- #
        u_x = apply_refractive_index(u_x, n_refraction, config)
        # -------------------------------------- #

        # ------------------------------ #
        # --- Free-space propagation --- #
        # ------------------------------ #
        if config.ground == 'Dielectric':
            raise ValueError(['Dielectric ground not yet available in SSF'])

        elif config.ground == 'PEC':

            # descending relief
            if diff_relief[ii_x - 1] < 0:
                # Add zeros below the field
                u_x = shift_relief(u_x, -diff_relief[ii_x - 1])

            # Add the image layer to apply the local image method #
            if config.polar == 'TE':
                # print('TE')
                # Propagate using DSF. The first point u_x[0] is always = 0
                u_x_dx[1:config.N_z] = dssf_one_step_sin(u_x[1:config.N_z], propagator_dssf[1:config.N_z])
            elif config.polar == 'TM':
                # print('TM')
                # Propagate using DCF
                u_x_dx = dssf_one_step_cos(u_x, propagator_dssf)

            # ascending relief
            if diff_relief[ii_x - 1] > 0:
                # Put zeros in the relief
                u_x_dx = shift_relief(u_x_dx, -diff_relief[ii_x - 1])
                # u_x_dx[0:diff_relief[ii_x - 1]] = 0.0

                # end of the loop
        elif config.ground == 'None':
            # print('No ground')

            # Propagate using SSF
            u_x_dx = dssf_one_step(u_x, propagator_dssf)

        else:
            raise ValueError(['Ground condition should be dielectric, PEC, or None'])
        # ---------- END --------------- #
        # --- Free-space propagation --- #
        # ------------------------------ #

        # --- refractivity applied twice 2/2 --- #
        u_x_dx = apply_refractive_index(u_x_dx, n_refraction, config)
        if config.turbulence == 'Y':
            phi_turbulent = genere_phi_turbulent(config)
            u_x_dx = apply_phi_turbulent(u_x_dx, phi_turbulent, config)

        # -------------------------------------- #

        # update u_x
        # an one dimensional array 
        # every loop i add to etotal
        u_x = u_x_dx
        # u_x = copy.copy(u_x_dx)
        
        # SAVE TO E_TOTAL WITH U_X_DX  
        #!!!!! MY CHANGE on peut directement recuperer u_x YC
        
        # store field as a wavelet decomposition
        
        #print(u_x)
        #print(pywt.wavedec(u_x, config.wv_family, 'per', config.wv_L))
        k0 = 2*np.pi*config.freq/cst.c
        x_current = -config.x_s + (ii_x + 1) * config.x_step
        
        # PROBLEM !!!
        # IT WORKS BUT NOT CORRECT 
        
        ii_relief = ii_vect_relief[ii_x]
        u_x_dx = shift_relief(u_x_dx, ii_relief)
        
        e_total[ii_x-1, :] = u_x_dx / np.sqrt(k0 * x_current) * np.exp(-1j * k0 * x_current)
        #wv_total[ii_x-1] = sparsify(pywt.wavedec(u_x_dx, config.wv_family, 'per', config.wv_L))
        
    return u_x_dx, wv_total, e_total
