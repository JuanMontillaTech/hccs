<template>
  <div class="container">
    <b-modal
      size="lg"
      title="Formulario de Concepto"
      v-model="ShowModalCreate"
      hide-footer
    >
      <div class="container">
        <div class="row">
          <div class="col-lg-12 col-md-12 col-sm-12">
            <b-form-group label="Descripción del concepto">
              <b-form-input
                v-model="concept.Description"
                size="sm"
                :state="$v.concept.Description.$error ? false : null"
                trim
              ></b-form-input>
              <p
                class="text-danger text-size-required m-0"
                v-if="$v.concept.Description.$error"
              >
                Nombre de la cuenta requerido.
              </p>
            </b-form-group>
          </div>
          <div class="col-lg-6 col-md-6 col-sm-12">
            <b-form-group label="Cuenta de Débito">
              <vueselect
                :options="LedgerAccounts"
                v-model="concept.CreditLedgerAccountId"
                :reduce="(row) => row.id"
                label="name"
              ></vueselect>
              <p
                class="text-danger text-size-required m-0"
                v-if="$v.concept.CreditLedgerAccountId.$error"
              >
                Cuenta de Débito requerida.
              </p>
            </b-form-group>
          </div>
          <div class="col-lg-6 col-md-6 col-sm-12">
            <b-form-group label="Cuenta de Crédito">
              <vueselect
                :options="LedgerAccounts"
                v-model="concept.DebitLedgerAccountId"
                :reduce="(row) => row.id"
                label="name"
              ></vueselect>
              <p
                class="text-danger text-size-required m-0"
                v-if="$v.concept.DebitLedgerAccountId.$error"
              >
                Cuenta de Crédito requerida.
              </p>
            </b-form-group>
          </div>
          <div class="row justify-content-end w-100 gx-2">
            <div class="col-2 p-2">
              <b-button
                variant="danger"
                class="w-100"
                @click="ShowModalCreate = !ShowModalCreate"
                >Cancelar</b-button
              >
            </div>
            <div class="col-3 p-2">
              <b-button
                class="w-100"
                style="background-color: #457b9d"
                @click="save()"
              >
                <span>Guardar</span>
              </b-button>
            </div>
          </div>
        </div>
      </div>
    </b-modal>

    <!-- Modal for Concept details -->
    <b-modal
      size="lg"
      title="Formulario de Concepto"
      v-model="ShowModalDetails"
      hide-footer
    >
      <div class="container">
        <div class="row">
          <div class="col-lg-12 col-md-12 col-sm-12">
            <b-form-group label="Descripción del concepto">
              <b-form-input
                v-model="concept.Description"
                size="sm"
                :state="$v.concept.Description.$error ? false : null"
                trim
                disabled
              ></b-form-input>
              <p
                class="text-danger text-size-required m-0"
                v-if="$v.concept.Description.$error"
              >
                Nombre de la cuenta requerido.
              </p>
            </b-form-group>
          </div>
          <div class="col-lg-6 col-md-6 col-sm-12">
            <b-form-group label="Cuenta de Débito">
              <vueselect
                :options="LedgerAccounts"
                v-model="concept.CreditLedgerAccountId"
                :reduce="(row) => row.id"
                label="name"
                disabled
              ></vueselect>
              <p
                class="text-danger text-size-required m-0"
                v-if="$v.concept.CreditLedgerAccountId.$error"
              >
                Cuenta de Débito requerida.
              </p>
            </b-form-group>
          </div>
          <div class="col-lg-6 col-md-6 col-sm-12">
            <b-form-group label="Cuenta de Crédito">
              <vueselect
                :options="LedgerAccounts"
                v-model="concept.DebitLedgerAccountId"
                :reduce="(row) => row.id"
                label="name"
                disabled
              ></vueselect>
              <p
                class="text-danger text-size-required m-0"
                v-if="$v.concept.DebitLedgerAccountId.$error"
              >
                Cuenta de Crédito requerida.
              </p>
            </b-form-group>
          </div>
          <div class="row justify-content-end w-100 gx-2">
            <div class="col-2 p-2">
              <b-button
                variant="danger"
                class="w-100"
                @click="ShowModalDetails = !ShowModalDetails"
                >Cancelar</b-button
              >
            </div>
            <div class="col-3 p-2">
              <b-button
                class="w-100"
                style="background-color: #457b9d"
                @click="save()"
                disabled
              >
                <span>Guardar</span>
              </b-button>
            </div>
          </div>
        </div>
      </div>
    </b-modal>
    <!-- Modal for edit Concept -->
    <b-modal
      size="lg"
      title="Formulario de Concepto"
      v-model="ShowModalEdit"
      hide-footer
    >
      <div class="container">
        <div class="row">
          <div class="col-lg-12 col-md-12 col-sm-12">
            <b-form-group label="Descripción del concepto">
              <b-form-input
                v-model="concept.Description"
                size="sm"
                :state="$v.concept.Description.$error ? false : null"
                trim
              ></b-form-input>
              <p
                class="text-danger text-size-required m-0"
                v-if="$v.concept.Description.$error"
              >
                Nombre de la cuenta requerido.
              </p>
            </b-form-group>
          </div>
          <div class="col-lg-6 col-md-6 col-sm-12">
            <b-form-group label="Cuenta de Débito">
              <vueselect
                :options="LedgerAccounts"
                v-model="concept.CreditLedgerAccountId"
                :reduce="(row) => row.id"
                label="name"
              ></vueselect>
              <p
                class="text-danger text-size-required m-0"
                v-if="$v.concept.CreditLedgerAccountId.$error"
              >
                Cuenta de Débito requerida.
              </p>
            </b-form-group>
          </div>
          <div class="col-lg-6 col-md-6 col-sm-12">
            <b-form-group label="Cuenta de Crédito">
              <vueselect
                :options="LedgerAccounts"
                v-model="concept.DebitLedgerAccountId"
                :reduce="(row) => row.id"
                label="name"
              ></vueselect>
              <p
                class="text-danger text-size-required m-0"
                v-if="$v.concept.DebitLedgerAccountId.$error"
              >
                Cuenta de Crédito requerida.
              </p>
            </b-form-group>
          </div>
          <div class="row justify-content-end w-100 gx-2">
            <div class="col-2 p-2">
              <b-button
                variant="danger"
                class="w-100"
                @click="ShowModalEdit = !ShowModalEdit"
                >Cancelar</b-button
              >
            </div>
            <div class="col-3 p-2">
              <b-button
                class="w-100"
                style="background-color: #457b9d"
                @click="edit()"
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
          <b-button
            class="btn btn-light btn-sm"
            @click="showConcept(props.row)"
          >
            <fa icon="eye"></fa>
          </b-button>
          <b-button
            class="btn btn-light btn-sm"
            @click="removeConcept(props.row.Id)"
          >
            <fa icon="trash"></fa>
          </b-button>
          <b-button
            class="btn btn-light btn-sm"
            @click="editConcept(props.row)"
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
import { required } from "vuelidate/lib/validators";
export default {
  name: "Concepto",
  layout: "TheSlidebar",
  data() {
    return {
      ShowModalCreate: false,
      ShowModalEdit: false,
      ShowModalDelete: false,
      ShowModalDetails: false,
      concept: {
        Description: null,
        CreditLedgerAccountId: null,
        DebitLedgerAccountId: null,
      },
      izitoastConfig: {
        position: "topRight",
      },
      LedgerAccounts: [],
      rows: [],
      columns: [
        {
          label: "Descripción",
          field: "Description",
        },
        // {
        //   label: "Cuenta de crédito",
        //   field: "CreditLedgerAccountId",
        // },
        // {
        //   label: "Cuenta de débito",
        //   field: "DebitLedgerAccountId",
        // },
        {
          label: "Acciones",
          field: "action",
        },
      ],
    };
  },
  validations: {
    concept: {
      Description: {
        required,
      },
      CreditLedgerAccountId: {
        required,
      },
      DebitLedgerAccountId: {
        required,
      },
    },
  },
  created() {
    this.GetAllRows();
    this.getLeaderAccount();
  },
  methods: {
    showModal() {
      this.ShowModalCreate = true;
      this.clearForm();
    },
    showConcept(concept) {
      this.concept = concept;
      this.ShowModalDetails = true;
    },
    editConcept(concept) {
      this.concept = concept;
      this.ShowModalEdit = true;
    },
    GetAllRows() {
      this.rows = [];
      this.$axios
        .get("https://localhost:44367/api/Concept/GetAll", {
          headers: {
            "Content-Type": "application/json",
          },
        })
        .then((response) => {
          console.log("resp", this.LedgerAccounts);
          response.data.data.map((concept) => {
            let objConcept = {
              Id: concept.id,
              Description: concept.description,
              CreditLedgerAccountId: concept.creditLedgerAccountId,
              DebitLedgerAccountId: concept.debitLedgerAccountId,
            };
            this.rows.push(objConcept);
          });
        })
        .catch((error) => {
          this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
        });
    },
    edit() {
      this.put(this.concept);
    },
    save() {
      this.$v.$touch();
      if (this.$v.$invalid) {
        this.$toast.error(
          "Por favor complete el formulario correctamente.",
          "ERROR",
          this.izitoastConfig
        );
      } else {
        this.ShowModalCreate = false;
        this.post(this.concept);
        this.clearForm();
      }
    },
    async getLeaderAccount() {
      let url = `https://localhost:44367/api/LedgerAccount/GetAll`;
      let result = null;
      this.$axios
        .get(url, {
          headers: {
            "Content-Type": "application/json",
          },
        })
        .then((response) => {
          result = response;
          this.LedgerAccounts = result.data.data;
        })
        .catch((error) => {
          result = error;
        });
    },
    async post(data) {
      return new Promise((resolve, reject) => {
        this.$axios
          .post("https://localhost:44367/api/Concept/Create", data, {
            headers: {
              "Content-Type": "application/json",
            },
          })
          .then((response) => {
            resolve(response);
            this.$toast.success(
              "El Concepto ha sido creado correctamente.",
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
          .put("https://localhost:44367/api/Concept/Update", data, {
            headers: {
              "Content-Type": "application/json",
            },
          })
          .then((response) => {
            resolve(response);
            this.$toast.success(
              "El Concepto ha sido actualizado correctamente.",
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
    async delete(id) {
      return new Promise((resolve, reject) => {
        this.$axios
          .delete(`https://localhost:44367/api/Concept/Delete/${id}`, {
            headers: {
              "Content-Type": "application/json",
            },
          })
          .then((response) => {
            resolve(response);
            this.$toast.success(
              "El Concepto ha sido eliminado correctamente.",
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
    removeConcept(id) {
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
                fetch(`https://localhost:44367/api/Concept/Delete/${id}`, {
                  method: "DELETE",
                })
                  .then((resp) => {
                    alert(
                      "EXITO: El Concepto ha sido eliminado correctamente."
                    );
                  })
                  .catch((error) => {
                    alert(error);
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
        }
      );
    },
    clearForm() {
      this.concept = {
        Description: null,
        CreditLedgerAccountId: null,
        DebitLedgerAccountId: null,
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
