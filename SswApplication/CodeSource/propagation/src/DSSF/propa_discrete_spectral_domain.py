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
# @package compute_spectral_propagator
# @author Remi Douvenot
# @date 26/04/2021
# @brief Create the propagator for DSSF
# @warning only verified for Ny=Nz and y_step = z_step
# @warning Evanescent waves are put at zero
##

from numpy import pi, linspace, sin, logical_not, sqrt, zeros_like, exp
from scipy.constants import c


def discrete_spectral_propagator(simulation_parameters, n_z):
    # simulation parameters
    k0 = 2*pi*simulation_parameters.freq/c
    step_x = simulation_parameters.x_step
    step_z = simulation_parameters.z_step

    # compute k_z = 2/step_z * sin(pi*q_z/N_z_tot)
    qz_list = linspace(-n_z/2, n_z/2 - 1, num=n_z, endpoint=True)
    kz2_list = (2 / step_z * sin(pi * qz_list / n_z)) ** 2  # version DSSF

    # compute k_x^2
    k_x2 = k0 ** 2 - kz2_list
    ind_prop = k_x2 >= 0
    ind_evan = logical_not(ind_prop)

    # compute k_x
    k_x = zeros_like(k_x2, dtype='complex')
    # propagating waves
    k_x[ind_prop] = sqrt(k_x2[ind_prop])
    # evanescent waves = Take the sqrt with positive imag part
    k_x[ind_evan] = -1j * sqrt(-k_x2[ind_evan])

    # the very DSSF propagator
    propagator_dssf = exp(-1j*(k_x-k0)*step_x)

    # evanescent waves put to zero
    # propagator_dssf[k_x2 < 0] = 0

    return propagator_dssf


##
# @package compute_spectral_propagator
# @author Remi Douvenot
# @date 13/07/2021
# @brief Create the propagator for SSF
# @warning only verified for Ny=Nz and y_step = z_step
##


def continuous_spectral_propagator(simulation_parameters, n_z):
    # simulation parameters
    k0 = 2*pi*simulation_parameters.freq/c
    step_x = simulation_parameters.x_step
    z_max = simulation_parameters.z_step * n_z

    # compute k_z = q_z * pi / z_max
    qz_list = linspace(-n_z, (n_z - 1), num=n_z, endpoint=True)
    # kz2_list = ( 2 / step_z * sin(pi * qz_list / (N_z_tot)) ) ** 2
    kz2_list = (qz_list * pi / z_max) ** 2

    # compute k_x^2
    k_x2 = k0 ** 2 - kz2_list
    ind_prop = k_x2 >= 0
    ind_evan = logical_not(ind_prop)

    # compute k_x
    k_x = zeros_like(k_x2, dtype=complex)
    # propagating waves
    k_x[ind_prop] = sqrt(k_x2[ind_prop])
    # evanescent waves = Take the sqrt with positive imag part
    k_x[ind_evan] = -1j * sqrt(-k_x2[ind_evan])

    # SSF propagator is exp(-j (k_x - k_0) Delta_x)
    propagator_ssf = exp(-1j*(k_x-k0)*step_x)
    # propagator_ssf[k_x2 < 0] = 0

    return propagator_ssf


def discrete_spectral_propagator_sin(simulation_parameters, n_z):
    # simulation parameters
    k0 = 2*pi*simulation_parameters.freq/c
    step_x = simulation_parameters.x_step
    step_z = simulation_parameters.z_step

    # compute k_z = 2/step_z * sin(pi*q_z/2N_z_tot)
    qz_list = linspace(0, n_z-1, num=n_z, endpoint=True)
    if simulation_parameters.polar == 'TE':
        kz2_list = (2 / step_z * sin(pi * qz_list / (2*n_z))) ** 2  # version DSSF
    elif simulation_parameters.polar == 'TM':
        kz2_list = (2 / step_z * sin(pi * qz_list / (2 * (n_z-1)))) ** 2  # version DSSF

    # compute k_x^2
    k_x2 = k0 ** 2 - kz2_list
    ind_prop = k_x2 >= 0
    ind_evan = logical_not(ind_prop)

    # compute k_x
    k_x = zeros_like(k_x2, dtype='complex')
    # propagating waves
    k_x[ind_prop] = sqrt(k_x2[ind_prop])
    # evanescent waves = Take the sqrt with positive imag part
    k_x[ind_evan] = -1j * sqrt(-k_x2[ind_evan])

    # the very DSSF propagator
    propagator_dssf = exp(-1j*(k_x-k0)*step_x)

    # evanescent waves put to zero
    # propagator_dssf[k_x2 < 0] = 0

    return propagator_dssf
