<template>
  <div>
    <!-- Modal for create a contact -->

    <b-modal
      size="lg"
      title="Formulario de Configuración de Reporte"
      header-bg-variant="#000"
      v-model="ShowModalCreate"
      hide-footer
    >
      <div class="container">
        <div class="row">
          <div class="col-sm-12 col-md-6">
            <b-form-group label="Título">
              <b-form-input
                v-model="configuration.title"
                size="sm"
                trim
              ></b-form-input>
              <p
                class="text-danger text-size-required m-0"
                v-if="$v.configuration.title.$error"
              >
                Campo requerido.
              </p>
            </b-form-group>
          </div>
          <div class="col-sm-12 col-md-6">
            <b-form-group label="Cuenta de Débito">
              <vueselect
                :options="LedgerAccountList"
                v-model="configuration.parameter"
                :reduce="(row) => row.id"
                label="name"
              ></vueselect>
              <p
                class="text-danger text-size-required m-0"
                v-if="$v.configuration.parameter.$error"
              >
                Campo requerido.
              </p>
            </b-form-group>
          </div>
          <div class="col-sm-12 col-md-6">
            <b-form-group label="Detalles">
              <b-form-input
                v-model="configuration.details"
                size="sm"
                :state="$v.configuration.details.$error ? false : null"
                trim
              ></b-form-input>
              <p
                class="text-danger text-size-required m-0"
                v-if="$v.configuration.details.$error"
              >
                Campo requerido.
              </p>
            </b-form-group>
          </div>
          <div class="col-sm-12 col-md-6">
            <b-form-group label="Criterio">
              <b-form-input
                v-model="configuration.criterion"
                size="sm"
                trim
              ></b-form-input>
              <p
                class="text-danger text-size-required m-0"
                v-if="$v.configuration.criterion.$error"
              >
                Campo requerido.
              </p>
            </b-form-group>
          </div>
          <div class="col-sm-12 col-md-6">
            <b-form-group label="Valor">
              <b-form-input
                v-model="configuration.value"
                size="sm"
                trim
              ></b-form-input>
              <p
                class="text-danger text-size-required m-0"
                v-if="$v.configuration.value.$error"
              >
                Campo requerido.
              </p>
            </b-form-group>
          </div>
          <div class="col-sm-12 col-md-6">
            <b-form-group label="Código">
              <b-form-input
                v-model="configuration.code"
                size="sm"
                :state="$v.configuration.code.$error ? false : null"
                trim
              ></b-form-input>
              <p
                class="text-danger text-size-required m-0"
                v-if="$v.configuration.code.$error"
              >
                Campo requerido.
              </p>
            </b-form-group>
          </div>
          <div class="col-sm-12 col-md-6">
            <b-form-group label="Indice">
              <b-form-input
                v-model="configuration.index"
                size="sm"
                :state="$v.configuration.index.$error ? false : null"
                trim
              ></b-form-input>
              <p
                class="text-danger text-size-required m-0"
                v-if="$v.configuration.index.$error"
              >
                Campo requerido.
              </p>
            </b-form-group>
          </div>
          <div class="row justify-content-end w-100">
            <div class="d-flex justify-content-end">
              <b-button-group class="mt-4 mt-md-0">
                <b-button
                  variant="danger"
                  class="btn"
                  @click="ShowModalCreate = !ShowModalCreate"
                >
                  <i class="bx bx-x"></i> Cerrar
                </b-button>
                <b-button variant="success" class="btn" @click="saveContact()">
                  <i class="bx bx-save"></i> Guardar
                </b-button>
              </b-button-group>
            </div>
          </div>
        </div>
      </div>
    </b-modal>

    <!-- Modal for show contact details -->
    <b-modal
      size="lg"
      title="Formulario de Configuración de Reporte"
      header-bg-variant="#000"
      v-model="ShowModaldetails"
      hide-footer
    >
      <div class="container">
        <div class="row">
          <div class="col-sm-12 col-md-6">
            <b-form-group label="Título">
              <b-form-input
                v-model="configuration.title"
                size="sm"
                trim
                disabled
              ></b-form-input>
              <p
                class="text-danger text-size-required m-0"
                v-if="$v.configuration.title.$error"
              >
                Campo requerido.
              </p>
            </b-form-group>
          </div>
          <div class="col-sm-12 col-md-6">
            <b-form-group label="Cuenta de Débito">
              <vueselect
                :options="LedgerAccountList"
                v-model="configuration.parameter"
                :reduce="(row) => row.id"
                label="name"
                disabled
              ></vueselect>
              <p
                class="text-danger text-size-required m-0"
                v-if="$v.configuration.parameter.$error"
              >
                Campo requerido.
              </p>
            </b-form-group>
          </div>
          <div class="col-sm-12 col-md-6">
            <b-form-group label="Detalles">
              <b-form-input
                v-model="configuration.details"
                size="sm"
                :state="$v.configuration.details.$error ? false : null"
                trim
                disabled
              ></b-form-input>
              <p
                class="text-danger text-size-required m-0"
                v-if="$v.configuration.details.$error"
              >
                Campo requerido.
              </p>
            </b-form-group>
          </div>
          <div class="col-sm-12 col-md-6">
            <b-form-group label="Criterio">
              <b-form-input
                v-model="configuration.criterion"
                size="sm"
                trim
                disabled
              ></b-form-input>
              <p
                class="text-danger text-size-required m-0"
                v-if="$v.configuration.criterion.$error"
              >
                Campo requerido.
              </p>
            </b-form-group>
          </div>
          <div class="col-sm-12 col-md-6">
            <b-form-group label="Valor">
              <b-form-input
                v-model="configuration.value"
                size="sm"
                trim
                disabled
              ></b-form-input>
              <p
                class="text-danger text-size-required m-0"
                v-if="$v.configuration.value.$error"
              >
                Campo requerido.
              </p>
            </b-form-group>
          </div>
          <div class="col-sm-12 col-md-6">
            <b-form-group label="Código">
              <b-form-input
                v-model="configuration.code"
                size="sm"
                :state="$v.configuration.code.$error ? false : null"
                trim
                disabled
              ></b-form-input>
              <p
                class="text-danger text-size-required m-0"
                v-if="$v.configuration.code.$error"
              >
                Campo requerido.
              </p>
            </b-form-group>
          </div>
          <div class="col-sm-12 col-md-6">
            <b-form-group label="Indice">
              <b-form-input
                v-model="configuration.index"
                size="sm"
                :state="$v.configuration.index.$error ? false : null"
                trim
                disabled
              ></b-form-input>
              <p
                class="text-danger text-size-required m-0"
                v-if="$v.configuration.index.$error"
              >
                Campo requerido.
              </p>
            </b-form-group>
          </div>
          <div class="modal-footer">
            <div class="col-3 p-2">
              <b-button-group class="mt-4 mt-md-0">
                <b-button
                  variant="danger"
                  class="btn"
                  @click="ShowModaldetails = !ShowModaldetails"
                >
                  <i class="bx bx-x"></i> Cerrar
                </b-button>
              </b-button-group>
            </div>
          </div>
        </div>
      </div>
    </b-modal>

    <b-modal
      size="lg"
      title="Formulario de Configuración de Reporte"
      header-bg-variant="#000"
      v-model="ShowModalEdit"
      hide-footer
    >
      <div class="container">
        <div class="row">
          <div class="col-sm-12 col-md-6">
            <b-form-group label="Título">
              <b-form-input
                v-model="configuration.title"
                size="sm"
                trim
              ></b-form-input>
              <p
                class="text-danger text-size-required m-0"
                v-if="$v.configuration.title.$error"
              >
                Campo requerido.
              </p>
            </b-form-group>
          </div>
          <div class="col-sm-12 col-md-6">
            <b-form-group label="Cuenta de Débito">
              <vueselect
                :options="LedgerAccountList"
                v-model="configuration.parameter"
                :reduce="(row) => row.id"
                label="name"
              ></vueselect>
              <p
                class="text-danger text-size-required m-0"
                v-if="$v.configuration.parameter.$error"
              >
                Campo requerido.
              </p>
            </b-form-group>
          </div>
          <div class="col-sm-12 col-md-6">
            <b-form-group label="Detalles">
              <b-form-input
                v-model="configuration.details"
                size="sm"
                :state="$v.configuration.details.$error ? false : null"
                trim
              ></b-form-input>
              <p
                class="text-danger text-size-required m-0"
                v-if="$v.configuration.details.$error"
              >
                Campo requerido.
              </p>
            </b-form-group>
          </div>
          <div class="col-sm-12 col-md-6">
            <b-form-group label="Criterio">
              <b-form-input
                v-model="configuration.criterion"
                size="sm"
                trim
              ></b-form-input>
              <p
                class="text-danger text-size-required m-0"
                v-if="$v.configuration.criterion.$error"
              >
                Campo requerido.
              </p>
            </b-form-group>
          </div>
          <div class="col-sm-12 col-md-6">
            <b-form-group label="Valor">
              <b-form-input
                v-model="configuration.value"
                size="sm"
                trim
              ></b-form-input>
              <p
                class="text-danger text-size-required m-0"
                v-if="$v.configuration.value.$error"
              >
                Campo requerido.
              </p>
            </b-form-group>
          </div>
          <div class="col-sm-12 col-md-6">
            <b-form-group label="Código">
              <b-form-input
                v-model="configuration.code"
                size="sm"
                :state="$v.configuration.code.$error ? false : null"
                trim
              ></b-form-input>
              <p
                class="text-danger text-size-required m-0"
                v-if="$v.configuration.code.$error"
              >
                Campo requerido.
              </p>
            </b-form-group>
          </div>
          <div class="col-sm-12 col-md-6">
            <b-form-group label="Indice">
              <b-form-input
                v-model="configuration.index"
                size="sm"
                :state="$v.configuration.index.$error ? false : null"
                trim
              ></b-form-input>
              <p
                class="text-danger text-size-required m-0"
                v-if="$v.configuration.index.$error"
              >
                Campo requerido.
              </p>
            </b-form-group>
          </div>

          <div class="row justify-content-end w-100">
            <div class="d-flex justify-content-end">
              <b-button-group class="mt-4 mt-md-0">
                <b-button
                  variant="danger"
                  class="btn"
                  @click="ShowModalEdit = !ShowModalEdit"
                >
                  <i class="bx bx-x"></i> Cerrar
                </b-button>
                <b-button variant="success" class="btn" @click="editContact()">
                  <i class="bx bx-save"></i> Guardar
                </b-button>
              </b-button-group>
            </div>
          </div>
        </div>
      </div>
    </b-modal>

    <nav class="navbar navbar-default">
      <div class="container-fluid">
        <div class="navbar-header">
          <h4>Listado de configuración de reportes</h4>
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
            @click="GetAllRows()"
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
          <b-button variant="light" size="sm" @click="showContact(props.row)">
            <i class="fas fa-eye"></i>
          </b-button>
          <b-button
            variant="danger"
            size="sm"
            @click="removeContact(props.row)"
          >
            <i class="fas fa-trash"></i>
          </b-button>
          <b-button
            variant="info"
            size="sm"
            @click="editContactModal(props.row)"
          >
            <i class="fas fa-edit"></i>
          </b-button>
        </span>
        <span v-else>
          {{ props.formattedRow[props.column.field] }}
        </span>
      </template>
    </vue-good-table>
  </div>
</template>

<script>
import { required, email } from "vuelidate/lib/validators";
export default {
  data() {
    return {
      ShowModalCreate: false,
      ShowModalEdit: false,
      ShowModalDelete: false,
      ShowModaldetails: false,
      DeleteStatus: false,
      LedgerAccountList: [],
      configuration: {
        title: "",
        parameter: "",
        details: "",
        criterion: "",
        value: "",
        code: "",
        index: "",
      },
      izitoastConfig: {
        position: "topRight",
      },
      columns: [
        {
          label: "Código",
          field: "code",
        },
        {
          label: "Título",
          field: "title",
        },
        {
          label: "Criterio ",
          field: "criterion",
        },
        {
          label: "Acciones",
          field: "action",
        },
      ],
      rows: [],
    };
  },
  validations: {
    configuration: {
      title: {
        required,
      },
      parameter: {
        required,
      },
      details: {
        required,
      },
      criterion: {
        required,
      },
      value: {
        required,
      },
      code: {
        required,
      },
      index: {
        required,
      },
    },
  },
  created() {
    this.GetAllRows();
    this.getListForSelect();
  },
  methods: {
    async getListForSelect() {
      let url = this.$store.state.URL + `LedgerAccount/GetAll`;
      let result = null;
      this.$axios
        .get(url, {
          headers: {
            "Content-Type": "application/json",
          },
        })
        .then((response) => {
          result = response;
          this.LedgerAccountList = result.data.data;
        })
        .catch((error) => {
          result = error;
        });
    },
    GetAllRows() {
      this.$axios
        .get(this.$store.state.URL + "ConfigurationReport/GetAll", {
          headers: {
            "Content-Type": "application/json",
          },
        })
        .then((response) => {
          this.rows = response.data.data;
        })
        .catch((error) => {
          this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
        });
    },
    showModal() {
      this.clearForm();
      this.ShowModalCreate = true;
    },
    saveContact() {
      this.$v.$touch();
      if (this.$v.$invalid) {
        this.$toast.error(
          "Por favor complete el formulario correctamente.",
          "ERROR",
          this.izitoastConfig
        );
      } else {
        this.ShowModalCreate = false;
        this.post(this.configuration);
      }
    },
    showContact(configuration) {
      this.configuration.title = configuration.title;
      this.configuration.index = configuration.index;
      this.configuration.parameter = configuration.parameter;
      this.configuration.details = configuration.details;
      this.configuration.criterion = configuration.criterion;
      this.configuration.value = configuration.value;
      this.configuration.code = configuration.code;
      this.configuration.index = configuration.index;
      this.ShowModaldetails = true;
    },
    removeContact(configuration) {
      this.delete(configuration.id);
      this.GetAllRows();
    },
    editContactModal(configuration) {
      this.ShowModalEdit = true;
      this.configuration = configuration;
    },
    editContact() {
      this.put(this.configuration);
    },
    async post(data) {
      return new Promise((resolve, reject) => {
        this.$axios
          .post(this.$store.state.URL + "ConfigurationReport/Create", data, {
            headers: {
              "Content-Type": "application/json",
            },
          })
          .then((response) => {
            resolve(response);
            this.$toast.success(
              "El Registro ha sido creado correctamente.",
              "EXITO",
              this.izitoastConfig
            );
            this.GetAllRows();
          })
          .catch((error) => {
            reject(error);
            this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
          });
      });
    },
    async put(data) {
      return new Promise((resolve, reject) => {
        this.$axios
          .put(this.$store.state.URL + "ConfigurationReport/Update", data, {
            headers: {
              "Content-Type": "application/json",
            },
          })
          .then((response) => {
            resolve(response);
            this.$toast.success(
              "El Registro ha sido actualizado correctamente.",
              "EXITO",
              this.izitoastConfig
            );
            this.GetAllRows();
            this.ShowModalEdit = false;
          })
          .catch((error) => {
            reject(error);
            this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
          });
      });
    },
    async delete(id) {
      let result = false;
      this.$toast.question(
        "Esta seguro que quiere eliminar esta registro?",
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

                axios
                  .delete(
                    this.$store.state.URL +
                      `ConfigurationReport/Delete?id=${id}`,
                    {
                      headers: {
                        Authorization: `Bearer ${localStorage.getItem(
                          "token"
                        )}`,
                      },
                    }
                  )
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
      this.GetAllRows();
    },
    clearForm() {
      this.configuration = {
        title: "",
        parameter: "",
        details: "",
        criterion: "",
        value: "",
        code: "",
        index: "",
      };
      this.ShowModalCreate = false;
    },
  },
};
</script>

<style>
.text-size-required {
  font-size: 12px;
}
</style>
