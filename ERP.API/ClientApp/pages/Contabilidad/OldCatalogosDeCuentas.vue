<template>
  <div class="container">
    <!-- Modal for create countable accounts -->
    <b-modal
      size="md"
      title="Formulario de Catálogo de cuentas"
      v-model="ShowModalCreate"
      hide-footer
    >
      <div class="container">
        <div class="row justify-content-center">
          <div class="col-sm-12 col-md-12 col-lg-12">
            <b-form-group label="Nombre">
              <b-form-input
                v-model="Account.name"
                size="sm"
                trim
              ></b-form-input>
              <p
                class="text-danger text-size-required m-0"
                v-if="$v.Account.name.$error"
              >
                Nombre requerido.
              </p>
            </b-form-group>
          </div>
          <div class="col-sm-12 col-md-12 col-lg-12">
            <b-form-group label="Código de la cuenta">
              <b-form-input
                v-model="Account.code"
                size="sm"
                trim
              ></b-form-input>
            </b-form-group>
          </div>
          <div class="col-sm-12 col-md-12 col-lg-12">
            <b-form-group label="Cuenta de la que dependiente">
             
              <vueselect
                :options="accountSelectList"
                v-model="Account.belongs"
                :reduce="(row) => row.id"
                label="name"
              ></vueselect>
            </b-form-group>
          </div>
          <div class="col-sm-12 col-md-12 col-lg-12">
            <b-form-group label="Descripción">
              <b-form-textarea v-model="Account.commentary"></b-form-textarea>
            </b-form-group>
          </div>
          <div class="container">
            <div class="row justify-content-end">
              <div class="col-sm-12 col-md-4 col-lg-4">
                <b-button
                  variant="danger"
                  class="w-100"
                  @click="ShowModalCreate = !ShowModalCreate"
                  >Cancelar</b-button
                >
              </div>
              <div class="col-sm-12 col-md-4 col-lg-4">
                <b-button
                  style="background-color: #457b9d"
                  class="w-100"
                  @click="saveAccount()"
                >
                  <span>Guardar</span>
                </b-button>
              </div>
            </div>
          </div>
        </div>
      </div>
    </b-modal>

    <!-- Modal for show countable accounts -->
    <b-modal
      size="md"
      title="Formulario de Catálogo de cuentas"
      v-model="ShowModalDetails"
      hide-footer
    >
      <div class="container">
        <div class="row justify-content-center">
          <div class="col-sm-12 col-md-12 col-lg-12">
            <b-form-group label="Nombre">
              <b-form-input
                v-model="Account.name"
                size="sm"
                trim
                disabled
              ></b-form-input>
            </b-form-group>
          </div>
          <div class="col-sm-12 col-md-12 col-lg-12">
            <b-form-group label="Código de la cuenta">
              <b-form-input
                v-model="Account.code"
                size="sm"
                trim
                disabled
              ></b-form-input>
            </b-form-group>
          </div>
          <div class="col-sm-12 col-md-12 col-lg-12">
            <b-form-group label="Cuenta de la que dependiente">
             
              <vueselect
                :options="accountSelectList"
                v-model="Account.belongs"
                :reduce="(row) => row.id"
                label="name"
                disabled
              ></vueselect>
            </b-form-group>
          </div>
          <div class="col-sm-12 col-md-12 col-lg-12">
            <b-form-group label="Descripción">
              <b-form-textarea
                v-model="Account.commentary"
                disabled
              ></b-form-textarea>
            </b-form-group>
          </div>
          <div class="container">
            <div class="row justify-content-end">
              <div class="col-sm-12 col-md-4 col-lg-4">
                <b-button
                  variant="danger"
                  class="w-100"
                  @click="ShowModalDetails = !ShowModalDetails"
                  >Cancelar</b-button
                >
              </div>
              <div class="col-sm-12 col-md-4 col-lg-4">
                <b-button
                  style="background-color: #457b9d"
                  class="w-100"
                  @click="saveAccount()"
                  disabled
                >
                  <span>Guardar</span>
                </b-button>
              </div>
            </div>
          </div>
        </div>
      </div>
    </b-modal>

    <!-- Modal for update countable accounts -->
    <b-modal
      size="md"
      title="Formulario de Catálogo de cuentas"
      v-model="ShowModalEdit"
      hide-footer
    >
      <div class="container">
        <div class="row justify-content-center">
          <div class="col-sm-12 col-md-12 col-lg-12">
            <b-form-group label="Nombre">
              <b-form-input
                v-model="Account.name"
                size="sm"
                trim
              ></b-form-input>
              <p
                class="text-danger text-size-required m-0"
                v-if="$v.Account.name.$error"
              >
                Nombre requerido.
              </p>
            </b-form-group>
          </div>
          <div class="col-sm-12 col-md-12 col-lg-12">
            <b-form-group label="Código de la cuenta">
              <b-form-input
                v-model="Account.code"
                size="sm"
                trim
              ></b-form-input>
            </b-form-group>
          </div>
          <div class="col-sm-12 col-md-12 col-lg-12">
            <b-form-group label="Cuenta de la que dependiente">
              <vueselect
                :options="accountSelectList"
                v-model="Account.belongs"
                :reduce="(row) => row.id"
                label="name"
              ></vueselect>
            </b-form-group>
          </div>
          <div class="col-sm-12 col-md-12 col-lg-12">
            <b-form-group label="Descripción">
              <b-form-textarea v-model="Account.commentary"></b-form-textarea>
            </b-form-group>
          </div>
          <div class="container">
            <div class="row justify-content-end">
              <div class="col-sm-12 col-md-4 col-lg-4">
                <b-button
                  variant="danger"
                  class="w-100"
                  @click="ShowModalEdit = !ShowModalEdit"
                  >Cancelar</b-button
                >
              </div>
              <div class="col-sm-12 col-md-4 col-lg-4">
                <b-button
                  style="background-color: #457b9d"
                  class="w-100"
                  @click="updateAccount()"
                >
                  <span>Guardar</span>
                </b-button>
              </div>
            </div>
          </div>
        </div>
      </div>
    </b-modal>

    <div>
      <nav class="navbar navbar-default">
        <div class="container-fluid">
          <div class="navbar-header">
            <div><h4>Listado de Catalogos De Cuentas</h4></div>
          </div>
          <div class="btn-group" role="group" aria-label="Basic example">
            <a
              title="Nuevo Registro"
              @click="showModal()"
              class="btn btn-primary btn-sm text-white"
            >
                <i class="fas fa-file"></i>
              Nuevo</a
            >
            <a
              id="_btnRefresh"
              @click="GetAllSchemaRows()"
              class="btn btn-light border btn-sm text-black-50 btnRefresh"
              name="_btnRefresh"
              ><i class="fas fa-sync-alt"></i> Actualizar Datos</a
            >
          </div>
        </div>
      </nav>
      <vue-good-table
        :columns="columns"
        :rows="rows"
        :search-options="{
          enabled: true,
        }"
        :pagination-options="{
          enabled: true,
          mode: 'records',
        }"
      >
        <template slot="table-row" slot-scope="props">
        <span v-if="props.column.field == 'action'">
          <b-button-group class="mt-4 mt-md-0">
            <b-button
              size="sm"
              variant="danger"
            @click="removeAccount(props.row.id)"
            >
              <i class="fas fa-trash"></i>
            </b-button>
            <b-button size="sm" variant="info"   @click="editAccount(props.row)">
              <i class="fas fa-edit"></i>
            </b-button>
          </b-button-group>
        </span>
        <span v-else>
          {{ props.formattedRow[props.column.field] }}
        </span>
      </template>
      
      </vue-good-table>
    </div>
  </div>
</template>

<script>
import axios from "axios";
import VJstree from "vue-jstree";
import { required } from "vuelidate/lib/validators";
export default {
  layout: "TheSlidebar",
  name: "CatalogosDeCuentas",
  components: {
    VJstree,
  },
  data() {
    return {
      Account: {
        belongs: null,
        name: null,
        code: null,
        commentary: null,
        nature: 0,
        locationStatusResult: 0,
      },
      izitoastConfig: {
        position: "topRight",
      },
      ShowModalCreate: false,
      ShowModalEdit: false,
      ShowModalDelete: false,
      ShowModalDetails: false,
      accountSelectList: [],
      columns: [
        {
          label: "Título",
          field: "name",
          width: "50px",
        },
        {
          label: "Descripción",
          field: "commentary",
          width: "50px",
        },
        {
          label: "Acciones",
          field: "action",
          width: "50px",
        },
      ],
      rows: [],
    };
  },
middleware: "authentication",

  created() {
    this.GetAllSchemaRows();
    this.getListForSelect();
  },
  validations: {
    Account: {
      name: {
        required,
      },
    },
  },
  methods: {
    async getListForSelect() {
      let url =   `LedgerAccount/GetAll`;
      let result = null;
      this.$axios
        .get(url )
        .then((response) => {
          result = response;
          this.accountSelectList = result.data.data;
        })
        .catch((error) => {
          this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
        });
    },
    showModal() {
      this.ShowModalCreate = true;
      this.clearForm();
    },
    GetAllSchemaRows() {
      this.rows = [];
      this.$axios
        .get(  "LedgerAccount/GetAll" )
        .then((response) => {
          response.data.data.map((schema) => {
            let objSchema = {
              id: schema.id,
              commentary: schema.commentary,
              name: schema.name,
              code: schema.code,
            };
            this.rows.push(objSchema);
          });
        })
        .catch((error) => {
          this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
        });
    },
    showAccount(account) {
      this.ShowModalDetails = true;
      this.Account = account;
    },
    editAccount(account) {
      this.ShowModalEdit = true;

      this.Account = account;
    },
    async removeAccount(id) {
      this.$toast.question(
        "Esta seguro que quiere eliminar esta cuenta?",
        "PREGUNTA",
        {
          timeout: 20000,
          close: false,
          overlay: true,
          toastOnce: true,
          id: "question",
          zindex: 999,
          position: "center",
          buttons: [
            [
              "<button><b>YES</b></button>",
              function (instance, toast) {
                instance.hide({ transitionOut: "fadeOut" }, toast, "button");
                let url =   `LedgerAccount/Delete/?id=${id}`;
               this.$axios
                  .delete(url )
                  .then((response) => {
                    alert(
                      "EXITO: El Registro ha sido eliminado correctamente."
                    );
                    location.reload();
                  })
                  .catch((error) => alert(error));
              },
              true,
            ],
            [
              "<button>NO</button>",
              function (instance, toast) {
                instance.hide({ transitionOut: "fadeOut" }, toast, "button");
              },
            ],
          ],
        }
      );
    },
    async saveAccount() {
      this.$v.$touch();
      if (this.$v.$invalid) {
        this.$toast.error(
          "Por favor complete el formulario correctamente.",
          "ERROR",
          this.izitoastConfig
        );
      } else {
        let url =   "LedgerAccount/Create";
        return new Promise((resolve, reject) => {
          this.$axios
            .post(url, this.Account )
            .then((response) => {
              resolve(response);
              this.$toast.success(
                "La cuenta ha sido creado correctamente.",
                "EXITO",
                this.izitoastConfig
              );
              this.ShowModalCreate = false;
              this.clearForm();
              this.GetAllSchemaRows();
            })
            .catch((error) => {
              reject(error);
              this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
            });
        });
      }
    },
    async updateAccount() {
      this.$v.$touch();
      if (this.$v.$invalid) {
        this.$toast.error(
          "Por favor complete el formulario correctamente.",
          "ERROR",
          this.izitoastConfig
        );
      } else {
        let url =   "LedgerAccount/Update";
        return new Promise((resolve, reject) => {
          this.$axios
            .put(url, this.Account)
            .then((response) => {
              resolve(response);
              this.$toast.success(
                "La Cuenta ha sido actualizada correctamente.",
                "EXITO",
                this.izitoastConfig
              );
              this.ShowModalEdit = false;
              this.clearForm();
              this.GetAllSchemaRows();
            })
            .catch((error) => {
              reject(error);
              this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
            });
        });
      }
    },
    clearForm() {
      this.Account = {
        belongs: null,
        name: null,
        code: null,
        commentary: null,
        nature: 0,
        locationStatusResult: 0,
      };
    },
  },
};
</script>

<style>
 
.text-size-required {
  font-size: 12px;
}
</style>
