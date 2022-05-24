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
            <b-form-group label="Refencia">
              <b-form-input
                v-model="concept.Reference"
                size="sm"
                :state="$v.concept.Reference.$error ? false : null"
                trim
              ></b-form-input>
              <p
                class="text-danger text-size-required m-0"
                v-if="$v.concept.Reference.$error"
              >
                Campo requerido.
              </p>
            </b-form-group>
          </div>
          <div class="col-lg-6 col-md-6 col-sm-12">
            <b-form-group label="Precio Compra">
              <b-form-input
                type="number"
                v-model="concept.PricePurchase"
                size="sm"
                :state="$v.concept.PricePurchase.$error ? false : null"
                trim
              ></b-form-input>
              <p
                class="text-danger text-size-required m-0"
                v-if="$v.concept.PricePurchase.$error"
              >
                Campo requerido.
              </p>
            </b-form-group>
          </div>
          <div class="col-lg-6 col-md-6 col-sm-12">
            <b-form-group label="Precio Venta">
              <b-form-input
                type="number"
                v-model="concept.PriceSale"
                size="sm"
                :state="$v.concept.PriceSale.$error ? false : null"
                trim
              ></b-form-input>
              <p
                class="text-danger text-size-required m-0"
                v-if="$v.concept.PriceSale.$error"
              >
                Campo requerido.
              </p>
            </b-form-group>
          </div>
          <div class="col-lg-12 col-md-12 col-sm-12">
            <b-form-group label="Descripción">
              <b-form-textarea
                v-model="concept.Description"
                size="sm"
                trim
              ></b-form-textarea>
            </b-form-group>
          </div>
          <div class="col-lg-12 col-md-12 col-sm-12">
            <b-form-group label="Observaciones">
              <b-form-textarea
                v-model="concept.Observations"
                size="sm"
                trim
              ></b-form-textarea>
            </b-form-group>
          </div>
          <div class="col-lg-12 col-md-12 col-sm-12 my-4">
            <b-form-checkbox
              v-model="concept.IsPurchase"
              :value="true"
              :unchecked-value="false"
            >
              Se compra
            </b-form-checkbox>
            <b-form-checkbox
              v-model="concept.ForSale"
              :value="true"
              :unchecked-value="false"
            >
              Se vende
            </b-form-checkbox>
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
                Campo requerido.
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
                Campo requerido.
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
            <b-form-group label="Refencia">
              <b-form-input
                v-model="concept.Reference"
                size="sm"
                :state="$v.concept.Reference.$error ? false : null"
                trim
                disabled
              ></b-form-input>
              <p
                class="text-danger text-size-required m-0"
                v-if="$v.concept.Reference.$error"
              >
                Campo requerido.
              </p>
            </b-form-group>
          </div>
          <div class="col-lg-6 col-md-6 col-sm-12">
            <b-form-group label="Precio Compra">
              <b-form-input
                type="number"
                v-model="concept.PricePurchase"
                size="sm"
                :state="$v.concept.PricePurchase.$error ? false : null"
                trim
                disabled
              ></b-form-input>
              <p
                class="text-danger text-size-required m-0"
                v-if="$v.concept.PricePurchase.$error"
              >
                Campo requerido.
              </p>
            </b-form-group>
          </div>
          <div class="col-lg-6 col-md-6 col-sm-12">
            <b-form-group label="Precio Venta">
              <b-form-input
                type="number"
                v-model="concept.PriceSale"
                size="sm"
                :state="$v.concept.PriceSale.$error ? false : null"
                trim
                disabled
              ></b-form-input>
              <p
                class="text-danger text-size-required m-0"
                v-if="$v.concept.PriceSale.$error"
              >
                Campo requerido.
              </p>
            </b-form-group>
          </div>
          <div class="col-lg-12 col-md-12 col-sm-12">
            <b-form-group label="Descripción">
              <b-form-textarea
                v-model="concept.Description"
                size="sm"
                trim
                disabled
              ></b-form-textarea>
            </b-form-group>
          </div>
          <div class="col-lg-12 col-md-12 col-sm-12">
            <b-form-group label="Observaciones">
              <b-form-textarea
                v-model="concept.Observations"
                size="sm"
                trim
                disabled
              ></b-form-textarea>
            </b-form-group>
          </div>
          <div class="col-lg-12 col-md-12 col-sm-12 my-4">
            <b-form-checkbox
              v-model="concept.IsPurchase"
              :value="true"
              :unchecked-value="false"
              disabled
            >
              Se compra
            </b-form-checkbox>
            <b-form-checkbox
              v-model="concept.ForSale"
              :value="true"
              :unchecked-value="false"
              disabled
            >
              Se vende
            </b-form-checkbox>
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
                Campo requerido.
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
                Campo requerido.
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
            <b-form-group label="Refencia">
              <b-form-input
                v-model="concept.Reference"
                size="sm"
                :state="$v.concept.Reference.$error ? false : null"
                trim
              ></b-form-input>
              <p
                class="text-danger text-size-required m-0"
                v-if="$v.concept.Reference.$error"
              >
                Campo requerido.
              </p>
            </b-form-group>
          </div>
          <div class="col-lg-6 col-md-6 col-sm-12">
            <b-form-group label="Precio Compra">
              <b-form-input
                type="number"
                v-model="concept.PricePurchase"
                size="sm"
                :state="$v.concept.PricePurchase.$error ? false : null"
                trim
              ></b-form-input>
              <p
                class="text-danger text-size-required m-0"
                v-if="$v.concept.PricePurchase.$error"
              >
                Campo requerido.
              </p>
            </b-form-group>
          </div>
          <div class="col-lg-6 col-md-6 col-sm-12">
            <b-form-group label="Precio Venta">
              <b-form-input
                type="number"
                v-model="concept.PriceSale"
                size="sm"
                :state="$v.concept.PriceSale.$error ? false : null"
                trim
              ></b-form-input>
              <p
                class="text-danger text-size-required m-0"
                v-if="$v.concept.PriceSale.$error"
              >
                Campo requerido.
              </p>
            </b-form-group>
          </div>
          <div class="col-lg-12 col-md-12 col-sm-12">
            <b-form-group label="Descripción">
              <b-form-textarea
                v-model="concept.Description"
                size="sm"
                trim
              ></b-form-textarea>
            </b-form-group>
          </div>
          <div class="col-lg-12 col-md-12 col-sm-12">
            <b-form-group label="Observaciones">
              <b-form-textarea
                v-model="concept.Observations"
                size="sm"
                trim
              ></b-form-textarea>
            </b-form-group>
          </div>
          <div class="col-lg-12 col-md-12 col-sm-12 my-4">
            <b-form-checkbox
              v-model="concept.IsPurchase"
              :value="true"
              :unchecked-value="false"
            >
              Se compra
            </b-form-checkbox>
            <b-form-checkbox
              v-model="concept.ForSale"
              :value="true"
              :unchecked-value="false"
            >
              Se vende
            </b-form-checkbox>
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
                Campo requerido.
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
                Campo requerido.
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
        Reference: null,
        CreditLedgerAccountId: null,
        DebitLedgerAccountId: null,
        Description: null,
        IsPurchase: null,
        ForSale: null,
        PricePurchase: null,
        PriceSale: null,
        Observations: null,
      },
      izitoastConfig: {
        position: "topRight",
      },
      LedgerAccounts: [],
      rows: [],
      columns: [
        {
          label: "Referencia",
          field: "Reference",
        },
        {
          label: "Descripción",
          field: "Description",
        },
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
      Reference: {
        required,
      },
      PricePurchase: {
        required,
      },
      PriceSale: {
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
        .get(process.env.devUrl + "Concept/GetAll", {
          headers: {
            "Content-Type": "application/json",
          },
        })
        .then((response) => {
          response.data.data.map((concept) => {
            let objConcept = {
              Id: concept.id,
              Description: concept.description,
              CreditLedgerAccountId: concept.creditLedgerAccountId,
              DebitLedgerAccountId: concept.debitLedgerAccountId,
              Reference: concept.reference,
              IsPurchase: concept.isPurchase,
              ForSale: concept.forSale,
              PricePurchase: concept.pricePurchase,
              PriceSale: concept.priceSale,
              Observations: concept.observations,
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
          this.LedgerAccounts = result.data.data;
        })
        .catch((error) => {
          result = error;
        });
    },
    async post(data) {
      return new Promise((resolve, reject) => {
        this.$axios
          .post(process.env.devUrl + "Concept/Create", data, {
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
          .put(process.env.devUrl + "Concept/Update", data, {
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
          .delete(process.env.devUrl + `Concept/Delete/${id}`, {
            headers: {
              "Content-Type": "application/json",
              Authorization: `Bearer ${localStorage.getItem("token")}`,
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
                fetch(process.env.devUrl + `Concept/Delete/${id}`, {
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
        Reference: null,
        CreditLedgerAccountId: null,
        DebitLedgerAccountId: null,
        Description: null,
        IsPurchase: null,
        ForSale: null,
        PricePurchase: null,
        PriceSale: null,
        Observations: null,
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
