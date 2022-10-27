<script>
/**
 * Login-1 component
 */
import Vue from "vue";
import Swal from "sweetalert2";

export default {
  layout: "auth",
  head() {
    return {
      title: `${this.title} | Sistema contable cardenal Sancha`,
    };
  },
  data() {
    return {
      title: "Ingreso al sistema",
      izitoastConfig: {
        position: "topRight",
      },
      userCredentials: {
        email: "",
        password: "",
      },
    };
  },
  created() {
    let api = process.env.PROD_API;
    if (process.env.NODE_ENV === "development") {
      api = process.env.DEV_API;
    }
    this.$axios.setBaseURL(api);
    this.$axios.setHeader("Content-Type", "application/json", ["post"]);
    this.$axios.setHeader("Content-Type", "application/json", ["put"]);
    this.$axios.setHeader("Content-Type", "application/json", ["get"]);
  },
  methods: {
    login() {
      this.showSpinnerLoading = true;
      this.$axios
        .post("Security/Login", this.userCredentials, {
          headers: {
            "Content-Type": "application/json",
          },
        })
        .then((response) => {
          if (response.data.succeeded) {
            const token = response.data.data;
            this.$axios.setToken(token, "Bearer");
            this.$axios.setHeader("Authorization", token);
            localStorage.setItem("authUser", token);
            localStorage.setItem("token", token);
            localStorage.setItem("Authorization", token);

            this.$router.push("/starter");
          } else {
            Swal.fire({
              icon: "error",
              title: "Oops...",
              text: response.data.friendlyMessage,
              footer: "Contactar al administrador",
            });
          }
        })
        .catch((error) => {
          console.log(error);
        })
        .finally(() => {});
    },
    // },
  },
};
</script>

<template>
  <div>
    <div class="home-btn d-none d-sm-block">
      <nuxt-link v-if="false" to="/" class="text-dark"
        ><i class="mdi mdi-home-variant h2"></i
      ></nuxt-link>
    </div>
    <div class="account-pages my-5 pt-sm-5">
      <div class="container">
        <div class="row">
          <div class="col-lg-12">
            <div class="text-center">
              <nuxt-link to="/" class="mb-5 d-block auth-logo">
                <img
                  src="~/assets/images/logo-dark.png"
                  alt=""
                  height="22"
                  class="logo logo-dark"
                />
                <img
                  src="~/assets/images/logo-light.png"
                  alt=""
                  height="22"
                  class="logo logo-light"
                />
              </nuxt-link>
            </div>
          </div>
        </div>
        <div class="row align-items-center justify-content-center">
          <div class="col-md-8 col-lg-6 col-xl-5">
            <div class="card">
              <div class="card-body p-4">
                <div class="text-center mt-2">
                  <h5 class="text-primary">Bienvenido de nuevo !</h5>
                  <p class="text-muted">Inicia sesión para continuar.</p>
                </div>
                <div class="p-2 mt-4">
                  <form>
                    <div class="mb-3">
                      <label for="username">Nombre de usuario</label>
                      <input
                        type="text"
                        class="form-control"
                        id="username"
                        placeholder="Usuario"
                        v-model="userCredentials.email"
                      />
                    </div>

                    <div class="mb-3">
                      <div v-if="false" class="float-end">
                        <nuxt-link to="/auth/recoverpwd" class="text-muted">
                          Forgot password?</nuxt-link
                        >
                      </div>
                      <label for="userpassword">Contraseña</label>
                      <input
                        type="password"
                        class="form-control"
                        id="userpassword"
                        v-model="userCredentials.password"
                        placeholder="Contraseña"
                      />
                    </div>

                    <div v-if="false" class="form-check">
                      <input
                        type="checkbox"
                        class="form-check-input"
                        id="auth-remember-check"
                      />
                      <label class="form-check-label" for="auth-remember-check"
                        >Remember me</label
                      >
                    </div>

                    <div class="mt-3 text-end">
                      <a
                        class="btn btn-primary w-sm waves-effect waves-light"
                        @click="login()"
                      >
                        Ingresar
                      </a>
                    </div>

                    <div class="mt-4 text-center" v-if="false">
                      <div class="signin-other-title">
                        <h5 class="font-size-14 mb-3 title">Sign in with</h5>
                      </div>

                      <ul class="list-inline">
                        <li class="list-inline-item">
                          <a
                            href="javascript:void()"
                            class="social-list-item bg-primary text-white border-primary"
                          >
                            <i class="mdi mdi-facebook"></i>
                          </a>
                        </li>
                        <li class="list-inline-item">
                          <a
                            href="javascript:void()"
                            class="social-list-item bg-info text-white border-info"
                          >
                            <i class="mdi mdi-twitter"></i>
                          </a>
                        </li>
                        <li class="list-inline-item">
                          <a
                            href="javascript:void()"
                            class="social-list-item bg-danger text-white border-danger"
                          >
                            <i class="mdi mdi-google"></i>
                          </a>
                        </li>
                      </ul>
                    </div>

                    <div v-if="false" class="mt-4 text-center">
                      <p class="mb-0">
                        Don't have an account ?
                        <nuxt-link
                          to="/auth/register-1"
                          class="fw-medium text-primary"
                        >
                          Signup now</nuxt-link
                        >
                      </p>
                    </div>
                  </form>
                </div>
              </div>
            </div>

            <div class="mt-5 text-center">
              <p>
                © {{ new Date().getFullYear() }} Creado con
                <i class="mdi mdi-heart text-danger"></i> por
                <a
                  class="text-reset"
                  href="https://api.whatsapp.com/send/?phone=18293087380&text=Hola Juan!&app_absent=0"
                  target="_blank"
                  ><i class="fab fa-whatsapp"> </i>Juan Montilla</a
                >
              </p>
            </div>
          </div>
        </div>
        <!-- end row -->
      </div>
      <!-- end container -->
    </div>
  </div>
</template>
