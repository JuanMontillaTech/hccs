<template>
  <div class="container">
    <!-- Modal for create countable Taxess -->
    <b-modal
      size="lg"
      title="Formulario de Impuestos"
      v-model="ShowModalCreate"
      hide-footer
    >
      <div class="container">
        <div class="row justify-content-center">
          <div class="col-sm-12 col-md-6 col-lg-6">
            <b-form-group label="Código del impuesto">
              <b-form-input v-model="Taxes.code" size="sm" trim></b-form-input>
            </b-form-group>
          </div>
          <div class="col-sm-12 col-md-6 col-lg-6">
            <b-form-group label="Nombre">
              <b-form-input v-model="Taxes.name" size="sm" trim></b-form-input>
              <p
                class="text-danger text-size-required m-0"
                v-if="$v.Taxes.name.$error"
              >
                Campo requerido.
              </p>
            </b-form-group>
          </div>
          <div class="col-sm-12 col-md-6 col-lg-6">
            <b-form-group label="Procentaje">
              <b-form-input
                type="number"
                v-model="Taxes.percentage"
                size="sm"
                trim
              ></b-form-input>
              <p
                class="text-danger text-size-required m-0"
                v-if="$v.Taxes.percentage.$error"
              >
                Campo requerido.
              </p>
            </b-form-group>
          </div>
          <div class="col-sm-12 col-md-6 col-lg-6">
            <b-form-group label="Tipo">
              <b-form-select
                v-model="Taxes.taxType"
                :options="optionstaxType"
              ></b-form-select>
              <p
                class="text-danger text-size-required m-0"
                v-if="$v.Taxes.taxType.$error"
              >
                Campo requerido.
              </p>
            </b-form-group>
          </div>
          <div class="col-sm-12 col-md-6 col-lg-6">
            <b-form-group label="Subcuenta de impuesto repercutido">
              <vueselect
                :options="TaxesSelectList"
                v-model="Taxes.CreditLedgerAccountId"
                :reduce="(row) => row.id"
                label="name"
              ></vueselect>
            </b-form-group>
          </div>
          <div class="col-sm-12 col-md-6 col-lg-6">
            <b-form-group label="Subcuenta impuesto soportado">
              <vueselect
                :options="TaxesSelectList"
                v-model="Taxes.DebitLedgerAccountId"
                :reduce="(row) => row.id"
                label="name"
              ></vueselect>
            </b-form-group>
          </div>
          <div class="col-sm-12 col-md-12 col-lg-12">
            <b-form-group label="Descripción">
              <b-form-textarea v-model="Taxes.description"></b-form-textarea>
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
                  @click="saveTaxes()"
                >
                  <span>Guardar</span>
                </b-button>
              </div>
            </div>
          </div>
        </div>
      </div>
    </b-modal>

    <!-- Modal for show countable Taxess -->
    <b-modal
      size="lg"
      title="Formulario de Impuestos"
      v-model="ShowModalDetails"
      hide-footer
    >
      <div class="container">
        <div class="row justify-content-center">
          <div class="col-sm-12 col-md-6 col-lg-6">
            <b-form-group label="Código del impuesto">
              <b-form-input
                v-model="Taxes.code"
                size="sm"
                trim
                disabled
              ></b-form-input>
            </b-form-group>
          </div>
          <div class="col-sm-12 col-md-6 col-lg-6">
            <b-form-group label="Nombre">
              <b-form-input
                v-model="Taxes.name"
                size="sm"
                trim
                disabled
              ></b-form-input>
              <p
                class="text-danger text-size-required m-0"
                v-if="$v.Taxes.name.$error"
              >
                Campo requerido.
              </p>
            </b-form-group>
          </div>
          <div class="col-sm-12 col-md-6 col-lg-6">
            <b-form-group label="Procentaje">
              <b-form-input
                type="number"
                v-model="Taxes.percentage"
                size="sm"
                trim
                disabled
              ></b-form-input>
              <p
                class="text-danger text-size-required m-0"
                v-if="$v.Taxes.percentage.$error"
              >
                Campo requerido.
              </p>
            </b-form-group>
          </div>
          <div class="col-sm-12 col-md-6 col-lg-6">
            <b-form-group label="Tipo">
              <b-form-select
                v-model="Taxes.taxType"
                :options="optionstaxType"
                disabled
              ></b-form-select>
              <p
                class="text-danger text-size-required m-0"
                v-if="$v.Taxes.taxType.$error"
              >
                Campo requerido.
              </p>
            </b-form-group>
          </div>
          <div class="col-sm-12 col-md-6 col-lg-6">
            <b-form-group label="Subcuenta de impuesto repercutido">
              <vueselect
                :options="TaxesSelectList"
                v-model="Taxes.CreditLedgerAccountId"
                :reduce="(row) => row.id"
                label="name"
                disabled
              ></vueselect>
            </b-form-group>
          </div>
          <div class="col-sm-12 col-md-6 col-lg-6">
            <b-form-group label="Subcuenta impuesto soportado">
              <vueselect
                :options="TaxesSelectList"
                v-model="Taxes.DebitLedgerAccountId"
                :reduce="(row) => row.id"
                label="name"
                disabled
              ></vueselect>
            </b-form-group>
          </div>
          <div class="col-sm-12 col-md-12 col-lg-12">
            <b-form-group label="Descripción">
              <b-form-textarea
                v-model="Taxes.description"
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
                  @click="saveTaxes()"
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

    <!-- Modal for update countable Taxess -->
    <b-modal
      size="lg"
      title="Formulario de Impuestos"
      v-model="ShowModalEdit"
      hide-footer
    >
      <div class="container">
        <div class="row justify-content-center">
          <div class="col-sm-12 col-md-6 col-lg-6">
            <b-form-group label="Código del impuesto">
              <b-form-input v-model="Taxes.code" size="sm" trim></b-form-input>
            </b-form-group>
          </div>
          <div class="col-sm-12 col-md-6 col-lg-6">
            <b-form-group label="Nombre">
              <b-form-input v-model="Taxes.name" size="sm" trim></b-form-input>
              <p
                class="text-danger text-size-required m-0"
                v-if="$v.Taxes.name.$error"
              >
                Campo requerido.
              </p>
            </b-form-group>
          </div>
          <div class="col-sm-12 col-md-6 col-lg-6">
            <b-form-group label="Procentaje">
              <b-form-input
                type="number"
                v-model="Taxes.percentage"
                size="sm"
                trim
              ></b-form-input>
              <p
                class="text-danger text-size-required m-0"
                v-if="$v.Taxes.percentage.$error"
              >
                Campo requerido.
              </p>
            </b-form-group>
          </div>
          <div class="col-sm-12 col-md-6 col-lg-6">
            <b-form-group label="Tipo">
              <b-form-select
                v-model="Taxes.taxType"
                :options="optionstaxType"
              ></b-form-select>
              <p
                class="text-danger text-size-required m-0"
                v-if="$v.Taxes.taxType.$error"
              >
                Campo requerido.
              </p>
            </b-form-group>
          </div>
          <div class="col-sm-12 col-md-6 col-lg-6">
            <b-form-group label="Subcuenta de impuesto repercutido">
              <vueselect
                :options="TaxesSelectList"
                v-model="Taxes.CreditLedgerAccountId"
                :reduce="(row) => row.id"
                label="name"
              ></vueselect>
            </b-form-group>
          </div>
          <div class="col-sm-12 col-md-6 col-lg-6">
            <b-form-group label="Subcuenta impuesto soportado">
              <vueselect
                :options="TaxesSelectList"
                v-model="Taxes.DebitLedgerAccountId"
                :reduce="(row) => row.id"
                label="name"
              ></vueselect>
            </b-form-group>
          </div>
          <div class="col-sm-12 col-md-12 col-lg-12">
            <b-form-group label="Descripción">
              <b-form-textarea v-model="Taxes.description"></b-form-textarea>
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
                  @click="updateTaxes()"
                >
                  <span>Guardar</span>
                </b-button>
              </div>
            </div>
          </div>
        </div>
      </div>
    </b-modal>
    <!-- updateTaxes() -->
    <div>
      <nav class="navbar navbar-default">
        <div class="container-fluid">
          <div class="navbar-header">
            <div>Listado de Impuestos</div>
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
            <b-button
              class="btn btn-light btn-sm"
              @click="showTaxes(props.row)"
            >
              <fa icon="eye"></fa>
            </b-button>
            <b-button
              class="btn btn-light btn-sm"
              @click="removeTaxes(props.row.id)"
            >
              <fa icon="trash"></fa>
            </b-button>
            <b-button
              class="btn btn-light btn-sm"
              @click="editTaxes(props.row)"
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
  </div>
</template>

<script>
import axios from "axios";
import VJstree from "vue-jstree";
import { required } from "vuelidate/lib/validators";
export default {
  layout: "TheSlidebar",
  name: "Impuestos",
  components: {
    VJstree,
  },
  data() {
    return {
      Taxes: {
        CreditLedgerAccountId: null,
        DebitLedgerAccountId: null,
        name: null,
        percentage: null,
        taxType: null,
        code: null,
        description: null,
      },
      optionstaxType: [
        { value: 1, text: "Porcentaje" },
        { value: 2, text: "Fijo" },
      ],
      izitoastConfig: {
        position: "topRight",
      },
      ShowModalCreate: false,
      ShowModalEdit: false,
      ShowModalDelete: false,
      ShowModalDetails: false,
      TaxesSelectList: [],
      columns: [
        {
          label: "Nombre",
          field: "code",
          width: "50px",
        },
        {
          label: "Descripción",
          field: "description",
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
  created() {
    this.GetAllSchemaRows();
    this.getListForSelect();
  },
  validations: {
    Taxes: {
      name: {
        required,
      },
      percentage: {
        required,
      },
      taxType: {
        required,
      },
    },
  },
  methods: {
    async getListForSelect() {
      let url =   `LedgerAccount/GetAll`;
      let result = null;
      this.$axios
        .get(url, {
          headers: {
            "Content-Type": "application/json",
            Authorization: `${localStorage.getItem("authUser")}`,
          },
        })
        .then((response) => {
          result = response;
          this.TaxesSelectList = result.data.data;
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
        .get(  "Taxes/GetAll", {
          headers: {
            "Content-Type": "application/json",
          },
        })
        .then((response) => {
          response.data.data.map((schema) => {
            let objSchema = {
              id: schema.id,
              code: schema.code,
              name: schema.name,
              taxType: schema.taxType,
              CreditLedgerAccountId: schema.creditLedgerAccountId,
              DebitLedgerAccountId: schema.debitLedgerAccountId,
              percentage: schema.percentage,
              description: schema.description,
            };
            this.rows.push(objSchema);
          });
        })
        .catch((error) => {
          this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
        });
    },
    showTaxes(Taxes) {
      this.ShowModalDetails = true;
      this.Taxes = Taxes;
    },
    editTaxes(Taxes) {
      this.ShowModalEdit = true;
      this.Taxes = Taxes;
    },
    async removeTaxes(id) {
      console.log(id);
      this.$toast.question(
        "Esta seguro que quiere eliminar este registro?",
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
                let url =   `Taxes/Delete/${id}`;
                // fetch(url, {
                //   method: "DELETE",
                // })
                //   .then((resp) => {
                //     location.reload();
                //   })
                //   .catch((error) => {
                //     alert(error);
                //   });
                axios
                  .delete(url, {
                    headers: {
                     Authorization: `${localStorage.getItem("authUser")}`,
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
    async saveTaxes() {
      this.$v.$touch();
      if (this.$v.$invalid) {
        this.$toast.error(
          "Por favor complete el formulario correctamente.",
          "ERROR",
          this.izitoastConfig
        );
      } else {
        let url =   "Taxes/Create";
        return new Promise((resolve, reject) => {
          this.$axios
            .post(url, this.Taxes, {
              headers: {
                "Content-Type": "application/json",
                Authorization: `${localStorage.getItem("authUser")}`,
              },
            })
            .then((response) => {
              resolve(response);
              this.$toast.success(
                "El registro ha sido creado correctamente.",
                "EXITO",
                this.izitoastConfig
              );
              this.ShowModalCreate = false;
              //   this.clearForm();
              this.GetAllSchemaRows();
            })
            .catch((error) => {
              reject(error);
              this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
            });
        });
      }
    },
    async updateTaxes() {
      this.$v.$touch();
      if (this.$v.$invalid) {
        this.$toast.error(
          "Por favor complete el formulario correctamente.",
          "ERROR",
          this.izitoastConfig
        );
      } else {
        let url =   "Taxes/Update";
        return new Promise((resolve, reject) => {
          this.$axios
            .put(url, this.Taxes, {
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
      this.Taxes = {
        CreditLedgerAccountId: null,
        DebitLedgerAccountId: null,
        name: null,
        percentage: null,
        taxType: null,
        code: null,
        description: null,
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
