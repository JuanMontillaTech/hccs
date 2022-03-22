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
                  style="background-color: #457b9d"
                  class="w-100"
                  @click="saveAccount()"
                  disabled
                >
                  <span>Crear</span>
                </b-button>
              </div>
              <div class="col-sm-12 col-md-4 col-lg-4">
                <b-button
                  variant="danger"
                  class="w-100"
                  @click="ShowModalDetails = !ShowModalDetails"
                  >Cancelar</b-button
                >
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
            <b-form-group label="Descripción">
              <b-form-textarea v-model="Account.commentary"></b-form-textarea>
            </b-form-group>
          </div>
          <div class="container">
            <div class="row justify-content-end">
              <div class="col-sm-12 col-md-4 col-lg-4">
                <b-button
                  style="background-color: #457b9d"
                  class="w-100"
                  @click="updateAccount()"
                >
                  <span>Crear</span>
                </b-button>
              </div>
              <div class="col-sm-12 col-md-4 col-lg-4">
                <b-button
                  variant="danger"
                  class="w-100"
                  @click="ShowModalEdit = !ShowModalEdit"
                  >Cancelar</b-button
                >
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
            <div>Listado de Catalogos De Cuentas</div>
          </div>
          <div class="btn-group" role="group" aria-label="Basic example">
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
          <span v-if="props.column.field == 'name'">
            <VJstree :data="data">
              <template scope="_">
                <div style="display: inherit; width: 200px">
                  <i
                    :class="_.vm.themeIconClasses"
                    role="presentation"
                    v-if="!_.model.loading"
                  ></i>
                  {{ _.model.text }}
                  <button
                    style="
                      border: 0px;
                      background-color: transparent;
                      cursor: pointer;
                    "
                    @click="showAccount(_.model)"
                  >
                    <i class="fa fa-eye"></i>
                  </button>
                  <button
                    style="
                      border: 0px;
                      background-color: transparent;
                      cursor: pointer;
                    "
                    @click="addAccount(_.model)"
                  >
                    <i class="fa fa-plus"></i>
                  </button>
                  <button
                    style="
                      border: 0px;
                      background-color: transparent;
                      cursor: pointer;
                    "
                    @click="editAccount(_.model)"
                  >
                    <i class="fa fa-edit"></i>
                  </button>
                  <button
                    style="
                      border: 0px;
                      background-color: transparent;
                      cursor: pointer;
                    "
                    @click="removeAccount(_.model)"
                  >
                    <i class="fa fa-trash"></i>
                  </button>
                </div>
              </template>
            </VJstree>

            <!-- <VJstree :data="data" show-checkbox multiple allow-batch whole-row @item-click="itemClick"></VJstree> -->
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
        belongs: "721686BC-3B86-4AFE-837F-D690C03251A8",
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
      data: [
        {
          id: 0,
          text: "Activos",
          icon: "fa fa-money text-success",
          accountCode: "0001",
          description:
            "Lorem, ipsum dolor sit amet consectetur adipisicing elit. Ullam, nostrum!",
          children: [
            {
              id: 1,
              text: "Activos circulantes",
              icon: "fa fa-money text-success",
              accountCode: "0002",
              description:
                "Lorem, ipsum dolor sit amet consectetur adipisicing elit. Ullam, nostrum!",
            },
            {
              id: 2,
              text: "Activos no circulantes",
              icon: "fa fa-money text-success",
              accountCode: "0003",
              description:
                "Lorem, ipsum dolor sit amet consectetur adipisicing elit. Ullam, nostrum!",
            },
          ],
        },
      ],

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
      ],
      rows: [],
    };
  },
  created() {
    this.GetAllSchemaRows();
  },
  validations: {
    Account: {
      name: {
        required,
      },
    },
  },
  methods: {
    GetAllSchemaRows() {
      this.rows = [];
      this.$axios
        .get("https://localhost:44367/api/LedgerAccount/GetAll", {
          headers: {
            "Content-Type": "application/json",
          },
        })
        .then((response) => {
          console.log(response);
          response.data.data.map((schema) => {
            let objSchema = {
              id: schema.id,
              commentary: schema.commentary,
              name: schema.name,
              code: schema.code,
            };
            // let objData = {
            //   id: schema.id,
            //   text: schema.name,
            //   icon: "fa fa-money text-success",
            //   code: schema.code,
            //   commentary: schema.commentary,
            //   nature: schema.nature,
            //   locationStatusResult: schema.locationStatusResult,
            //   children: [],
            // };
            this.rows.push(objSchema);
            // this.data.push(objData);
          });
        })
        .catch((error) => {
          this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
        });
    },
    convertJsonToObj(data) {
      const obj = {};
      for (let [key, value] of Object.entries(data)) {
        obj[key] = value;
      }
      return obj;
    },
    showAccount(node) {
      this.ShowModalDetails = true;
      const resp = this.convertJsonToObj(node);
      this.Account.Name = resp.text;
      this.Account.AccountCode = resp.accountCode;
      this.Account.Description = resp.description;
      console.log(resp);
    },
    addAccount(node) {
      this.ShowModalCreate = true;
      const resp = this.convertJsonToObj(node);
      console.log(resp);
    },
    editAccount(node) {
      this.ShowModalEdit = true;
      const resp = this.convertJsonToObj(node);
      this.Account.Name = resp.text;
      this.Account.AccountCode = resp.accountCode;
      this.Account.Description = resp.description;
      console.log(resp);
    },
    async removeAccount(node) {
      const resp = this.convertJsonToObj(node);
      console.log(resp);
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
                const id = resp.id;
                console.log(id);
                let url = `/${id}`;
                return new Promise((resolve, reject) => {
                  this.$axios
                    .delete(url, {
                      headers: {
                        "Content-Type": "application/json",
                      },
                    })
                    .then((response) => {
                      resolve(response);
                      this.$toast.success(
                        "La Cuenta ha sido eliminada correctamente.",
                        "EXITO",
                        this.izitoastConfig
                      );
                    })
                    .catch((error) => {
                      reject(error);
                      this.$toast.error(
                        `${error}`,
                        "ERROR",
                        this.izitoastConfig
                      );
                    });
                });
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
          onClosing: function (instance, toast, closedBy) {
            console.info("Closing | closedBy: " + closedBy);
          },
          onClosed: function (instance, toast, closedBy) {
            console.info("Closed | closedBy: " + closedBy);
          },
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
        let url = "https://localhost:44367/api/LedgerAccount/Create";
        return new Promise((resolve, reject) => {
          this.$axios
            .post(url, this.Account, {
              headers: {
                "Content-Type": "application/json",
              },
            })
            .then((response) => {
              resolve(response);
              console.log(response);
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
      console.log(this.Account);
      this.$v.$touch();
      if (this.$v.$invalid) {
        this.$toast.error(
          "Por favor complete el formulario correctamente.",
          "ERROR",
          this.izitoastConfig
        );
      } else {
        let url = "https://localhost:44367/api/LedgerAccount/Update";
        return new Promise((resolve, reject) => {
          this.$axios
            .put(url, this.Account, {
              headers: {
                "Content-Type": "application/json",
              },
            })
            .then((response) => {
              resolve(response);
              this.$toast.success(
                "La Cuenta ha sido actualizada correctamente.",
                "EXITO",
                this.izitoastConfig
              );
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
        belongs: "721686BC-3B86-4AFE-837F-D690C03251A8",
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
.modal-header {
  background-color: #457b9d !important;
  color: #fff;
}
.text-size-required {
  font-size: 12px;
}
</style>
