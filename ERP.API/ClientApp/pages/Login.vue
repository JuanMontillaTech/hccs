<template>
    <div class="container-fluid">
        <div class="row" style="display: flex; justify-content: center; align-items: center; height: 100vh">
            <div class="d-flex w-50 border shadow p-3 mb-5 bg-body rounded">
                <div class="col-md-6 ads" style="color: #fff; background-color:#1d3557; ">
                    <h6 class="loginText m-4">
                            El sistema puede presentar error ya que estamos incorporando nuevas funcionalidades 
                    </h6>
                    <img src="../assets/images/warningBuilding.jpg" alt="Cardenal Sanchas" height="300px" width="300px;">

                </div>
                <div class="col-md-6 login-form">
                    <div class="d-flex justify-content-center img">
                        <img src="../assets/images/warningBuilding.jpg" alt="Cardenal Sanchas" height="140px" width="140px;">
                    </div>

                    <h3 class="text-center my-3" style="font-size:20px;">
                        El sistema puede presentar error ya que estamos incorporando nuevas funcionalidades
                    </h3>
                    
                    <form data-role="validator" method="post" data-clear-invalid="3000">
                        <div class="form-group">
                            <input type="text" class="form-control" name="Correo" placeholder="Usuario/Correo" v-model="userCredentials.email">
                            <p class="text-danger text-size-required m-0" v-if="!$v.userCredentials.email.required">Usuario o Correo requerido.</p>
                        </div>
                        <div class="form-group">
                            <input type="password" class="form-control" name="contrasena" placeholder="Contraseña" v-model="userCredentials.password">
                            <p class="text-danger text-size-required m-0" v-if="!$v.userCredentials.password.required">Contraseña requerido.</p>
                        </div>

                        <div class="form-group">
                            <!-- <button class="btn btn-info btn-lg btn-block" style="color: #fff; background-color:#1d3557;"  @click="login()">Ingresar</button> -->
                            <b-button style="color: #fff; background-color:#1d3557;" class="w-100" @click="login()">Send</b-button>
                        </div>
                    </form>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
import { required } from "vuelidate/lib/validators";
import { Login } from '../api/Login/LoginService'
export default {
    name: 'Login',
    layout: 'default',
    data(){
        return {
            userCredentials: {
                email: null,
                password: null
            },
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
            if (this.$v.$invalid) {
                console.log('error!')
            } else {
                const response = await Login(this.userCredentials);
                console.log(response);
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