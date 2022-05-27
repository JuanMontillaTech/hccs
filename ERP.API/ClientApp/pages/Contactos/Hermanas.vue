<template>
  <div>
    <!-- Modal for create a contact -->

    <b-modal
      size="lg"
      title="Formulario de Hermana"
      header-bg-variant="#000"
      v-model="ShowModalCreate"
      hide-footer
    >
      <div class="container">
        <div class="row">
          <div class="col-sm-12 col-md-6">
            <b-form-group label="Tipo de identificación">
              <b-form-select
                v-model="contact.identificationType"
                :options="options"
                size="sm"
              ></b-form-select>
            </b-form-group>
          </div>
          <div class="col-sm-12 col-md-6">
            <b-form-group label="Número Documento">
              <b-form-input
                v-model="contact.documentNumber"
                size="sm"
                trim
              ></b-form-input>
            </b-form-group>
          </div>
          <div class="col-sm-12 col-md-6">
            <b-form-group label="Nombre">
              <b-form-input
                v-model="contact.name"
                size="sm"
                :state="$v.contact.name.$error ? false : null"
                trim
              ></b-form-input>
              <p
                class="text-danger text-size-required m-0"
                v-if="$v.contact.name.$error"
              >
                Nombre requerido.
              </p>
            </b-form-group>
          </div>
          <div class="col-sm-12 col-md-6">
            <b-form-group label="Municipio / Provincia">
              <b-form-select
                v-model="contact.provinceId"
                :options="provinces"
                size="sm"
              ></b-form-select>
            </b-form-group>
          </div>
          <div class="col-sm-12 col-md-6">
            <b-form-group label="Salario">
              <b-form-input
                v-model="contact.salary"
                size="sm"
                trim
              ></b-form-input>
            </b-form-group>
          </div>
          <div class="col-sm-12 col-md-6">
            <b-form-group label="Dirección">
              <b-form-textarea
                v-model="contact.address"
                size="sm"
                trim
              ></b-form-textarea>
            </b-form-group>
          </div>
          <div class="col-sm-12 col-md-6">
            <b-form-group label="Correo electrónico">
              <b-form-input
                v-model="contact.email"
                size="sm"
                :state="$v.contact.email.$error ? false : null"
                trim
              ></b-form-input>
              <p
                class="text-danger text-size-required m-0"
                v-if="$v.contact.email.$error"
              >
                Formato de email incorrecto.
              </p>
            </b-form-group>
          </div>
          <div class="col-sm-12 col-md-6">
            <b-form-group label="Celular">
              <b-form-input
                v-model="contact.cellPhone"
                size="sm"
                trim
              ></b-form-input>
            </b-form-group>
          </div>
          <div class="col-sm-12 col-md-6">
            <b-form-group label="Teléfono 1">
              <b-form-input
                v-model="contact.phone1"
                size="sm"
                trim
              ></b-form-input>
            </b-form-group>
          </div>
          <div class="col-sm-12 col-md-6">
            <b-form-group label="Teléfono 2">
              <b-form-input
                v-model="contact.phone2"
                size="sm"
                trim
              ></b-form-input>
            </b-form-group>
          </div>

          <div class="row justify-content-end w-100 mt-4">
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
      title="Formulario de Hermana"
      v-model="ShowModalDetails"
      hide-footer
    >
      <div class="container">
        <div class="row">
          <div class="col-sm-12 col-md-6">
            <b-form-group label="Tipo de identificación">
              <b-form-select
                v-model="contact.identificationType"
                :options="options"
                size="sm"
                disabled
              ></b-form-select>
            </b-form-group>
          </div>
          <div class="col-sm-12 col-md-6">
            <b-form-group label="Número Documento">
              <b-form-input
                v-model="contact.documentNumber"
                size="sm"
                trim
                disabled
              ></b-form-input>
            </b-form-group>
          </div>
          <div class="col-sm-12 col-md-6">
            <b-form-group label="Nombre">
              <b-form-input
                v-model="contact.name"
                size="sm"
                :state="$v.contact.name.$error ? false : null"
                trim
                disabled
              ></b-form-input>
              <p
                class="text-danger text-size-required m-0"
                v-if="$v.contact.name.$error"
              >
                Nombre requerido.
              </p>
            </b-form-group>
          </div>
          <div class="col-sm-12 col-md-6">
            <b-form-group label="Municipio / Provincia">
              <b-form-select
                v-model="contact.provinceId"
                :options="provinces"
                size="sm"
                disabled
              ></b-form-select>
            </b-form-group>
          </div>
          <div class="col-sm-12 col-md-6">
            <b-form-group label="Salario">
              <b-form-input
                v-model="contact.salary"
                size="sm"
                trim
                disabled
              ></b-form-input>
            </b-form-group>
          </div>
          <div class="col-sm-12 col-md-6">
            <b-form-group label="Dirección">
              <b-form-textarea
                v-model="contact.address"
                size="sm"
                trim
                disabled
              ></b-form-textarea>
            </b-form-group>
          </div>
          <div class="col-sm-12 col-md-6">
            <b-form-group label="Correo electrónico">
              <b-form-input
                v-model="contact.email"
                size="sm"
                :state="$v.contact.email.$error ? false : null"
                trim
                disabled
              ></b-form-input>
              <p
                class="text-danger text-size-required m-0"
                v-if="$v.contact.email.$error"
              >
                Formato de email incorrecto.
              </p>
            </b-form-group>
          </div>
          <div class="col-sm-12 col-md-6">
            <b-form-group label="Celular">
              <b-form-input
                v-model="contact.cellPhone"
                size="sm"
                trim
                disabled
              ></b-form-input>
            </b-form-group>
          </div>
          <div class="col-sm-12 col-md-6">
            <b-form-group label="Teléfono 1">
              <b-form-input
                v-model="contact.phone1"
                size="sm"
                trim
                disabled
              ></b-form-input>
            </b-form-group>
          </div>
          <div class="col-sm-12 col-md-6">
            <b-form-group label="Teléfono 2">
              <b-form-input
                v-model="contact.phone2"
                size="sm"
                trim
                disabled
              ></b-form-input>
            </b-form-group>
          </div>

          <div class="row justify-content-end w-100 gx-2">
            <div class="col-2 p-2">
              <b-button
                variant="danger"
                class="w-100"
                @click="ShowModalDetails = !ShowModalDetails"
                >Cerrar</b-button
              >
            </div>
          </div>
        </div>
      </div>
    </b-modal>

    <!-- Modal for update contact -->
    <b-modal
      size="lg"
      title="Formulario de Hermana"
      v-model="ShowModalEdit"
      hide-footer
    >
      <div class="container">
        <div class="row">
          <div class="col-sm-12 col-md-6">
            <b-form-group label="Tipo de identificación">
              <b-form-select
                v-model="contact.identificationType"
                :options="options"
                size="sm"
              ></b-form-select>
            </b-form-group>
          </div>
          <div class="col-sm-12 col-md-6">
            <b-form-group label="Número Documento">
              <b-form-input
                v-model="contact.documentNumber"
                size="sm"
                trim
              ></b-form-input>
            </b-form-group>
          </div>
          <div class="col-sm-12 col-md-6">
            <b-form-group label="Nombre">
              <b-form-input
                v-model="contact.name"
                size="sm"
                :state="$v.contact.name.$error ? false : null"
                trim
              ></b-form-input>
              <p
                class="text-danger text-size-required m-0"
                v-if="$v.contact.name.$error"
              >
                Nombre social requerido.
              </p>
            </b-form-group>
          </div>
          <div class="col-sm-12 col-md-6">
            <b-form-group label="Municipio / Provincia">
              <b-form-select
                v-model="contact.provinceId"
                :options="provinces"
                size="sm"
              ></b-form-select>
            </b-form-group>
          </div>
          <div class="col-sm-12 col-md-6">
            <b-form-group label="Salario">
              <b-form-input
                v-model="contact.salary"
                size="sm"
                trim
              ></b-form-input>
            </b-form-group>
          </div>
          <div class="col-sm-12 col-md-6">
            <b-form-group label="Dirección">
              <b-form-textarea
                v-model="contact.address"
                size="sm"
                trim
              ></b-form-textarea>
            </b-form-group>
          </div>
          <div class="col-sm-12 col-md-6">
            <b-form-group label="Correo electrónico">
              <b-form-input
                v-model="contact.email"
                size="sm"
                :state="$v.contact.email.$error ? false : null"
                trim
              ></b-form-input>
              <p
                class="text-danger text-size-required m-0"
                v-if="$v.contact.email.$error"
              >
                Formato de email incorrecto.
              </p>
            </b-form-group>
          </div>
          <div class="col-sm-12 col-md-6">
            <b-form-group label="Celular">
              <b-form-input
                v-model="contact.cellPhone"
                size="sm"
                trim
              ></b-form-input>
            </b-form-group>
          </div>
          <div class="col-sm-12 col-md-6">
            <b-form-group label="Teléfono 1">
              <b-form-input
                v-model="contact.phone1"
                size="sm"
                trim
              ></b-form-input>
            </b-form-group>
          </div>
          <div class="col-sm-12 col-md-6">
            <b-form-group label="Teléfono 2">
              <b-form-input
                v-model="contact.phone2"
                size="sm"
                trim
              ></b-form-input>
            </b-form-group>
          </div>

          <div class="row justify-content-end w-100 mt-4">
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
          <h4>Listado de Hermanas</h4>
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
import axios from "axios";
import { required, email } from "vuelidate/lib/validators";
import Vue from "vue";
export default {
  data() {
    return {
      ShowModalCreate: false,
      ShowModalEdit: false,
      ShowModalDelete: false,
      ShowModalDetails: false,
      DeleteStatus: false,
      contact: {
        identificationType: "",
        documentNumber: "",
        name: "",
        address: "",
        provinceId: "",
        email: "",
        cellPhone: "",
        phone1: "",
        phone2: "",
        salary: "",
        isClient: false,
        isSupplier: false,
        isEmployee: false,
        isSister: true,
      },
      izitoastConfig: {
        position: "topRight",
      },
      provinces: [
        {
          value: 1,
          text: "Distrito Nacional",
        },
        {
          value: 21,
          text: "San Pedro de Macorís",
        },
        {
          value: 22,
          text: "La Romana",
        },
        {
          value: 23,
          text: "La Altagracia",
        },
        {
          value: 24,
          text: "El Seibo",
        },
        {
          value: 25,
          text: "Hato Mayor",
        },
        {
          value: 31,
          text: "Azua",
        },
        {
          value: 32,
          text: "Samaná",
        },
        {
          value: 33,
          text: "Maria Trinidad Sánchez",
        },
        {
          value: 34,
          text: "Salcedo",
        },
        {
          value: 41,
          text: "La Vega",
        },
        {
          value: 42,
          text: "Monseñor Nouel",
        },
        {
          value: 43,
          text: "Sánchez Ramirez",
        },
        {
          value: 51,
          text: "Santiago",
        },
        {
          value: 56,
          text: "Espaillat",
        },
        {
          value: 57,
          text: "Puerto Plata",
        },
        {
          value: 61,
          text: "Valverde",
        },
        {
          value: 62,
          text: "Monte Cristi",
        },
        {
          value: 63,
          text: "Dajabónn",
        },
        {
          value: 64,
          text: "Santiago Rodríguez",
        },
        {
          value: 71,
          text: "Azua",
        },
        {
          value: 72,
          text: "San Juan de la Maguana",
        },
        {
          value: 73,
          text: "Elías Piña",
        },
        {
          value: 81,
          text: "Barahona",
        },
        {
          value: 82,
          text: "Bahoruco",
        },
        {
          value: 83,
          text: "Independencia",
        },
        {
          value: 84,
          text: "Perdenales",
        },
        {
          value: 91,
          text: "San Cristóbal",
        },
        {
          value: 92,
          text: "Monte Plata",
        },
        {
          value: 93,
          text: "San José de Ocoa",
        },
        {
          value: 94,
          text: "Peravia",
        },
      ],
      selected: null,
      options: [
        { value: "1", text: "RNC" },
        { value: "2", text: "Cédula" },
        { value: "3", text: "Pasaporte (Identificador extranjero)" },
      ],
      columns: [
        {
          label: "Nombre",
          field: "name",
        },
        {
          label: "Identificación",
          field: "documentNumber",
          type: "number",
        },
        {
          label: "Télefono ",
          field: "cellPhone",
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
    contact: {
      name: {
        required,
      },
      email: {
        email,
      },
    },
  },
  created() {
    this.GetAllRows();
  },
  methods: {
    GetAllRows() {
      this.$axios
        .get(this.$store.state.URL + "Contact/GetAll", {
          headers: {
            "Content-Type": "application/json",
          },
        })
        .then((response) => {
          this.rows = response.data.data.filter(
            (person) => person.isSister == true
          );
        })
        .catch((error) => {
          this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
        });
    },
    showModal() {
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
        this.post(this.contact);
        this.clearForm();
      }
    },
    showContact(contact) {
      this.contact = contact;
      this.ShowModalDetails = true;
    },
    removeContact(contact) {
      this.delete(contact.id);
      this.GetAllRows();
    },
    editContactModal(contact) {
      this.ShowModalEdit = true;
      this.contact = contact;
    },
    editContact() {
      this.put(this.contact);
    },
    async post(data) {
      return new Promise((resolve, reject) => {
        this.$axios
          .post(this.$store.state.URL + "Contact/Create", data, {
            headers: {
              "Content-Type": "application/json",
            },
          })
          .then((response) => {
            resolve(response);
            this.$toast.success(
              "El registro ha sido creado correctamente.",
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
          .put(this.$store.state.URL + "Contact/Update", data, {
            headers: {
              "Content-Type": "application/json",
            },
          })
          .then((response) => {
            resolve(response);
            this.$toast.success(
              "El registro ha sido actualizado correctamente.",
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
                  .delete(this.$store.state.URL + `Contact/Delete?id=${id}`, {
                    headers: {
                      Authorization: `Bearer ${localStorage.getItem("token")}`,
                    },
                  })
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
      (this.contact = {
        identificationType: "",
        documentNumber: "",
        name: "",
        address: "",
        provinceId: "",
        email: "",
        cellPhone: "",
        phone1: "",
        phone2: "",
        salary: "",
        isClient: false,
        isSupplier: false,
        isEmployee: false,
        isSister: true,
      }),
        (this.ShowModalCreate = false);
    },
  },
};
</script>

<style>
.text-size-required {
  font-size: 12px;
}
</style>
