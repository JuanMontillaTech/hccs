<template>
    <div >
        <section class="vh-100" style="background-color: #457b9d;" @keyup.enter="login()">
            <div class="container py-5 h-100">
                <div class="row d-flex justify-content-center align-items-center h-100">
                <div class="col-12 col-md-8 col-lg-6 col-xl-5">
                    <div class="card shadow-2-strong" style="border-radius: 1rem;">
                    <div class="card-body p-5 text-center">
                        <h3 class="mb-5">Login</h3>

                        <div class="form-outline mb-4">
                        <input type="email" id="typeEmailX-2" class="form-control form-control-lg" placeholder="Usuario/Correo" v-model.trim="$v.userCredentials.email.$model" />
                        <div class="row mx-1">
                            <p class="text-danger text-size-required m-0" v-if="$v.userCredentials.email.$error">Usuario o Correo requerido.</p>
                        </div>
                        </div>

                        <div class="form-outline mb-4">
                        <input type="password" id="typePasswordX-2" class="form-control form-control-lg" placeholder="Contraseña" v-model="userCredentials.password" />
                        <div class="row mx-1">
                            <p class="text-danger text-size-required m-0" v-if="$v.userCredentials.password.$error">Contraseña requerido.</p>
                        </div>
                        </div>
                        <b-button :disabled="submitStatus === 'PENDING'" style="background-color: #457b9d;" class="btn-lg btn-block" @click="login()">
                            <span v-if="!showSpinnerLoading">Ingresar</span> 
                            <b-spinner label="Loading..." v-if="showSpinnerLoading"></b-spinner>
                        </b-button>
                    </div>
                    </div>
                </div>
                </div>
            </div>
        </section>
    </div>
</template>

<script>
 
import { required } from "vuelidate/lib/validators";
import { Login } from '../api/Login/LoginService'
import axios from 'axios';

export default {
    name: 'Login',
    data(){
        return {
            userCredentials: {
                email: '',
                password: ''
            },
            submitStatus: null,
            showSpinnerLoading: false,
            izitoastConfig: {
                position: 'topRight'
            }
        }
    },
    validations: {
        userCredentials: {
            email: {
                required
            },
            password: {
                required
            }
        }
    },
    methods: {
        async login() {
            this.$v.$touch();
            if (this.$v.$invalid) {
                this.$toast.warning('Por favor complete el formulario correctamente.', 'NOTIFICACIÓN', this.izitoastConfig);
            } 
            else {
                this.showSpinnerLoading = true;
                axios.post('https://localhost:44367/api/Security/Login', this.userCredentials)
                    .then((response) => {
                        if (response.data.succeeded) {
                            const token = response.data.data;
                            this.$toast.success(`Usuario ${this.userCredentials.email}`, 'BIENVENIDO', this.izitoastConfig);
                            this.$router.push('/Home');
                            localStorage.setItem('token', token);
                        } else {
                            this.$toast.info(response.data.friendlyMessage, 'NOTIFICACIÓN', this.izitoastConfig);
                        }
                    })
                    .catch((error) => {
                        this.$toast.error(error, 'ERROR', this.izitoastConfig);
                        console.log(error);
                    })
                    .finally(() => {
                        this.showSpinnerLoading = false;
                    })
            }
        },
    }
}
</script>

<style>
    img {
        border-radius: 50%;
    }
    .loginText {
        color: #fff; background-color:#1d3557; text-align: center; font-size:18px; width:250px;
    }
    .text-size-required {
        font-size: 12px;
    }
</style>