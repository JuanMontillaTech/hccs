<template>
  <div class="container">
    <b-modal
      size="lg"
      title="Formulario de Almacen"
      v-model="ShowModalCreate"
      hide-footer
    >
      <div class="container">
        <div class="row">
          <div class="col-lg-6 col-md-6 col-sm-12">
            <b-form-group label="Nombre">
              <b-form-input
                v-model="schema.name"
                size="sm"
                :state="$v.schema.name.$error ? false : null"
                trim
              ></b-form-input>
              <p
                class="text-danger text-size-required m-0"
                v-if="$v.schema.name.$error"
              >
                Campo requerido.
              </p>
            </b-form-group>
          </div>
          <div class="col-lg-6 col-md-6 col-sm-12">
            <b-form-group label="Dirección">
              <b-form-input
                v-model="schema.direction"
                size="sm"
                trim
              ></b-form-input>
            </b-form-group>
          </div>
          <div class="col-lg-12 col-md-12 col-sm-12">
            <b-form-group label="Observaciones">
              <b-form-textarea
                v-model="schema.observations"
                rows="3"
                max-rows="6"
                trim
              ></b-form-textarea>
            </b-form-group>
          </div>
          <div class="row justify-content-end w-100 gx-2">
            <div class="col-2 p-2">
              <b-button
                variant="danger"
                class="w-100"
                @click="ShowModalCreate = !ShowModalCreate"
                >Cerrar</b-button
              >
            </div>
            <div class="col-3 p-2">
              <b-button
                class="w-100"
                style="background-color: #457b9d"
                @click="saveSchema()"
              >
                <span>Guardar</span>
              </b-button>
            </div>
          </div>
        </div>
      </div>
    </b-modal>

    <!-- Modal for schema details -->
    <b-modal
      size="lg"
      title="Formulario de Almacen"
      v-model="ShowModalDetails"
      hide-footer
    >
      <div class="container">
        <div class="row">
          <div class="col-lg-6 col-md-6 col-sm-12">
            <b-form-group label="Nombre">
              <b-form-input
                v-model="schema.name"
                size="sm"
                :state="$v.schema.name.$error ? false : null"
                trim
                disabled
              ></b-form-input>
              <p
                class="text-danger text-size-required m-0"
                v-if="$v.schema.name.$error"
              >
                Campo requerido.
              </p>
            </b-form-group>
          </div>
          <div class="col-lg-6 col-md-6 col-sm-12">
            <b-form-group label="Dirección">
              <b-form-input
                v-model="schema.direction"
                size="sm"
                trim
                disabled
              ></b-form-input>
            </b-form-group>
          </div>
          <div class="col-lg-12 col-md-12 col-sm-12">
            <b-form-group label="Observaciones">
              <b-form-textarea
                v-model="schema.observations"
                rows="3"
                max-rows="6"
                trim
              ></b-form-textarea>
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
            <div class="col-3 p-2">
              <b-button
                class="w-100"
                style="background-color: #457b9d"
                @click="saveSchema()"
                disabled
              >
                <span>Guardar</span>
              </b-button>
            </div>
          </div>
        </div>
      </div>
    </b-modal>
    <!-- Modal for edit schema -->
    <b-modal
      size="lg"
      title="Formulario de Almacen"
      v-model="ShowModalEdit"
      hide-footer
    >
      <div class="container">
        <div class="row">
          <div class="col-lg-6 col-md-6 col-sm-12">
            <b-form-group label="Nombre">
              <b-form-input
                v-model="schema.name"
                size="sm"
                :state="$v.schema.name.$error ? false : null"
                trim
              ></b-form-input>
              <p
                class="text-danger text-size-required m-0"
                v-if="$v.schema.name.$error"
              >
                Campo requerido.
              </p>
            </b-form-group>
          </div>
          <div class="col-lg-6 col-md-6 col-sm-12">
            <b-form-group label="Dirección">
              <b-form-input
                v-model="schema.direction"
                size="sm"
                trim
              ></b-form-input>
            </b-form-group>
          </div>
          <div class="col-lg-12 col-md-12 col-sm-12">
            <b-form-group label="Observaciones">
              <b-form-textarea
                v-model="schema.observations"
                rows="3"
                max-rows="6"
                trim
              ></b-form-textarea>
            </b-form-group>
          </div>
          <div class="row justify-content-end w-100 gx-2">
            <div class="col-2 p-2">
              <b-button
                variant="danger"
                class="w-100"
                @click="ShowModalEdit = !ShowModalEdit"
                >Cerrar</b-button
              >
            </div>
            <div class="col-3 p-2">
              <b-button
                class="w-100"
                style="background-color: #457b9d"
                @click="editSchema()"
              >
                <span>Guardar</span>
              </b-button>
            </div>
          </div>
        </div>
      </div>
    </b-modal>
    <nav class="navbar navbar-default">
      <div class="container-fluid">
        <div class="navbar-header">
          <div>Listado de {{ $options.name }}</div>
        </div>
        <div class="btn-group" role="group" aria-label="Basic example">
          <a
            title="Nuevo Registro"
            @click="showModal()"
            class="btn btn-primary btn-sm text-white"
          >
            <fa icon="file" class="ml-1"></fa>
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
          <b-button class="btn btn-light btn-sm" @click="showSchema(props.row)">
            <fa icon="eye"></fa>
          </b-button>
          <b-button
            class="btn btn-light btn-sm"
            @click="removeSchema(props.row.id)"
          >
            <fa icon="trash"></fa>
          </b-button>
          <b-button
            class="btn btn-light btn-sm"
            @click="editModalSchema(props.row)"
          >
            <fa icon="edit"></fa>
            ></b-button
          >
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
import { required } from "vuelidate/lib/validators";
export default {
  name: "Almacenes",
  layout: "TheSlidebar",
  data() {
    return {
      ShowModalCreate: false,
      ShowModalEdit: false,
      ShowModalDelete: false,
      ShowModalDetails: false,
      schema: {
        id: null,
        name: null,
        direction: null,
        observations: null,
      },
      izitoastConfig: {
        position: "topRight",
      },
      schemaSelectList: [],
      rows: [
        {
          id: 1,
          name: "Name 1",
          direction: "Lorem ipsum",
          observations: "Lorem ipsum observations",
        },
        {
          id: 2,
          name: "Name 2",
          direction: "Lorem ipsum",
          observations: "Lorem ipsum observations",
        },
        {
          id: 3,
          name: "Name 3",
          direction: "Lorem ipsum",
          observations: "Lorem ipsum observations",
        },
        {
          id: 4,
          name: "Name 4",
          direction: "Lorem ipsum",
          observations: "Lorem ipsum observations",
        },
        {
          id: 5,
          name: "Name 5",
          direction: "Lorem ipsum",
          observations: "Lorem ipsum observations",
        },
      ],
      columns: [
        {
          label: "Nombre",
          field: "name",
        },
        {
          label: "Dirección",
          field: "direction",
        },
        {
          label: "Observaciones",
          field: "observations",
        },
        {
          label: "Acciones",
          field: "action",
        },
      ],
    };
  },
  validations: {
    schema: {
      name: {
        required,
      },
    },
  },
  created() {
    this.GetAllSchemaRows();
    this.getListForSelect();
  },
  methods: {
    showModal() {
      this.ShowModalCreate = true;
      this.clearForm();
    },
    showSchema(schema) {
      this.schema = schema;
      this.ShowModalDetails = true;
    },
    editModalSchema(schema) {
      this.schema = schema;
      this.ShowModalEdit = true;
    },
    GetAllSchemaRows() {
      //   this.rows = [];
      this.$axios
        .get(process.env.devUrl + "schema/GetAll", {
          headers: {
            "Content-Type": "application/json",
          },
        })
        .then((response) => {
          response.data.data.map((schema) => {
            let objSchema = {
              id: schema.id,
              name: null,
              direction: null,
              observations: null,
            };
            // this.rows.push(objSchema);
          });
        })
        .catch((error) => {
          this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
        });
    },
    editSchema() {
      this.put(this.schema);
    },
    saveSchema() {
      this.$v.$touch();
      if (this.$v.$invalid) {
        this.$toast.error(
          "Por favor complete el formulario correctamente.",
          "ERROR",
          this.izitoastConfig
        );
      } else {
        this.ShowModalCreate = false;
        this.post(this.schema);
        this.clearForm();
      }
    },
    async getListForSelect() {
      let url = process.env.devUrl + `LedgerAccount/GetAll`;
      let result = null;
      this.$axios
        .get(url, {
          headers: {
            "Content-Type": "application/json",
          },
        })
        .then((response) => {
          result = response;
          this.schemaSelectList = result.data.data;
        })
        .catch((error) => {
          result = error;
        });
    },
    async post(data) {
      return new Promise((resolve, reject) => {
        this.$axios
          .post(process.env.devUrl + "schema/Create", data, {
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
            this.GetAllSchemaRows();
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
          .put(process.env.devUrl + "schema/Update", data, {
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
            this.ShowModalEdit = false;
          })
          .catch((error) => {
            reject(error);
            this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
          });
      });
    },
    removeSchema(id) {
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
                // fetch(process.env.devUrl + `schema/Delete/?id=${id}`, {
                //   method: "DELETE",
                // })
                //   .then((resp) => {
                //     alert(
                //       "EXITO: El Registro ha sido eliminado correctamente."
                //     );
                //   })
                //   .catch((error) => {
                //     alert(error);
                //   });
                axios
                  .delete(process.env.devUrl + `schema/Delete/?id=${id}`, {
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
    },
    clearForm() {
      this.schema = {
        id: null,
        name: null,
        direction: null,
        observations: null,
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
