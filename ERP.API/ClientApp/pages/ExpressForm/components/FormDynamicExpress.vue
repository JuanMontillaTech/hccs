<template>
  <div>
    <h4>{{ this.DataForm.title }}</h4>
    <div class="text-center" v-if="Spinning">
      <b-spinner variant="success" label="Spinning"></b-spinner>
    </div>

    <div class="row" v-if="Spinning == false">
      <div class="col-lg-12">
        <div class="alert alert-light" role="alert">
          <div v-if="$route.query.Action == 'edit'">
            <b-button-group>
              <b-button
                variant="secundary"
                class="btn"
                @click="GoBack()"
                v-if="DataForm.backList"
                size="sm"
              >
                <i class="bx bx-arrow-back"></i> Lista
              </b-button>
              <b-button
                variant="success"
                class="btn"
                @click="editSchema()"
                size="sm"
              >
                <i class="bx bx-save"></i> Actualizar
              </b-button>
            </b-button-group>
          </div>
          <div v-else>
            <b-button-group>
              <b-button
                variant="secundary"
                class="btn"
                @click="GoBack()"
                size="sm"
              >
                <i class="bx bx-arrow-back"></i> Lista
              </b-button>
              <b-button variant="success" @click="post()" size="sm">
                <i class="bx bx-save"></i> Crear
              </b-button>
            </b-button-group>
          </div>
        </div>
        <div class="card">
          <div class="card-body">
       
            <div
              v-for="(SectionRow, SectionIndex) in DataFormSection"
              :key="SectionIndex"
            >
               
              <table class="table table-bordered table-striped">
                <thead>
                  <tr>
                    <th>  <span
                    style="
                      font-size: 16px;
                      font-family: Georgia, 'Times New Roman', Times, serif;
                      font: bold;
                    "
                    >{{ SectionRow.name }}</span
                  ></th>
                  </tr>
                </thead>
                <tbody>
                <div
                  class="mb-auto p-1"
                  v-for="(fieldsRow, fieldIndex) in GetFilterDataOnlyshowForm(
                    SectionRow.fields
                  )"
                  :key="fieldIndex"
                >
                  <DynamicElementGrid
                    @CustomChange="GetValueFormElement"
                    :FieldsData="form"
                    :item="fieldsRow"
                    :labelShow="true"
                  ></DynamicElementGrid>
           
              </div>
            </tbody>
              </table>
            </div>

            <div class="row ml-0 mb-3" v-if="DataForm.upload">
              <div class="large-4 medium-4 small-4 cell">
                <div class="mb-3">
                  <span
                    style="
                      font-size: 16px;
                      font-family: Georgia, 'Times New Roman', Times, serif;
                      font: bold;
                    "
                    >Subir Archivos</span
                  >

                  <input
                    class="form-control"
                    type="file"
                    id="file"
                    ref="file"
                    multiple="multiple"
                  />
                </div>
              </div>
            </div>
            <div class="row ml-0 mb-3" v-if="DataForm.upload">
              <div class="large-4 medium-4 small-4 cell">
                <div class="mb-3">
                  <div class="table-responsive mb-0">
                    <b-table
                      :items="files"
                      :fields="filesTitle"
                      responsive="sm"
                    >
                      <template #cell(Acciones)="data">
                        <ul class="list-inline mb-0">
                          <li class="list-inline-item">
                            <a :href="data.item.link" target="_blank">{{
                              data.item.name
                            }}</a>
                          </li>

                          <li class="list-inline-item">
                            <b-button
                              size="sm"
                              variant="danger"
                              @click="confirmCancellation(data.item.id)"
                            >
                              <i class="fas fa-trash"></i>
                            </b-button>
                          </li>
                        </ul>
                      </template>
                    </b-table>
                  </div>
                </div>
              </div>
            </div>
            <table class="table" v-if="this.DataForm.transactionsType === 100">
              <thead class="bg-Cprimary">
                <tr>
                  <th style="width: 20%">
                    <template>
                      <b-button variant="primary" @click="addRow()">
                        <span> <i class="fas fa-plus"></i> </span>
                      </b-button>
                    </template>
                    Cuenta contable
                  </th>
                  <th style="width: 35%">Descripción</th>
                  <th style="width: 20%">Débito</th>
                  <th style="width: 20%">Crédito</th>
                  <th style="width: 5%"></th>
                </tr>
              </thead>
              <tbody>
                <tr
                  v-for="(JournalDetail, index) in form.journaDetails"
                  v-bind:key="index"
                  v-if="JournalDetail.isActive"
                >
                  <td>
                    <vueselect
                      :options="LedgerAccountes"
                      v-model="JournalDetail.ledgerAccountId"
                      :reduce="(row) => row.id"
                      label="name"
                    ></vueselect>
                  </td>
                  <td>
                    <textarea
                      v-model="JournalDetail.commentary"
                      class="form-control"
                      id="exampleFormControlTextarea1"
                      rows="3"
                    ></textarea>
                  </td>
                  <td>
                    <input
                      name="JournalDetail.debit"
                      v-model="JournalDetail.debit"
                      type="text"
                      v-on:keydown="GetTotal"
                      v-on:keyup="GetTotal"
                      class="form-control"
                    />
                  </td>
                  <td>
                    <input
                      v-model="JournalDetail.credit"
                      type="text"
                      v-on:keydown="GetTotal"
                      v-on:keyup="GetTotal"
                      class="form-control"
                      style="width: 60%"
                    />
                  </td>

                  <td>
                    <b-button-group class="mt-4 mt-md-0">
                      <b-button
                        size="sm"
                        variant="danger"
                        @click="removeRow(JournalDetail)"
                        :disabled="$route.query.action == 'show'"
                      >
                        <i class="fas fa-trash"></i>
                      </b-button>
                      <b-button
                        size="sm"
                        variant="info"
                        @click="addRow()"
                        :disabled="$route.query.action == 'show'"
                      >
                        <i class="fas fa-plus"></i>
                      </b-button>
                    </b-button-group>
                  </td>
                </tr>
              </tbody>
              <tfoot>
                <tr>
                  <td></td>
                  <td></td>
                  <td>{{ Tdebit }}</td>
                  <td>{{ Tcredit }}</td>
                </tr>
              </tfoot>
            </table>

            <div class="row ml-0 mb-3">
              <div class="col-lg-12 col-md-12 col-sm-12">
                <hr class="new1" />
                <b-form-group id="input-group-2" label-for="input-2">
                  <span
                    style="
                      font-size: 14px;
                      font-family: Georgia, 'Times New Roman', Times, serif;
                      font: bold;
                    "
                    >Comentario</span
                  >
                  <b-form-textarea
                    id="textarea"
                    v-model="principalSchema.commentary"
                    rows="3"
                    max-rows="6"
                  >
                  </b-form-textarea>
                </b-form-group>
              </div>
            </div>
            <div class="row ml-0 mb-3">
              <div class="col-lg-12 col-md-12 col-sm-12">
                <span
                  style="
                    font-size: 16px;
                    font-family: Georgia, 'Times New Roman', Times, serif;
                    font: bold;
                  "
                  >Auditoría</span
                >
                <hr class="new1" />
              </div>
            </div>
            <div class="row ml-0 mb-3">
              <div class="col-lg-6 col-md-6 col-sm-6">
                <b-form-group
                  id="input-group-2"
                  label="Creado Por :"
                  label-for="input-2"
                >
                  <b-form-input
                    id="textarea"
                    v-model="principalSchema.createdBy"
                    rows="3"
                    disabled
                    max-rows="6"
                  >
                  </b-form-input>
                </b-form-group>
              </div>
              <div class="col-lg-6 col-md-6 col-sm-6">
                <b-form-group
                  id="input-group-2"
                  label="Creado en :"
                  label-for="input-2"
                >
                  <b-form-input
                    id="textarea"
                    v-model="principalSchema.createdDate"
                    rows="3"
                    disabled
                    max-rows="6"
                  >
                  </b-form-input>
                </b-form-group>
              </div>
            </div>
            <div class="row ml-0 mb-3">
              <div class="col-lg-6 col-md-6 col-sm-6">
                <b-form-group
                  id="input-group-2"
                  label="Modificado Por:"
                  label-for="input-2"
                >
                  <b-form-input
                    id="textarea"
                    v-model="principalSchema.createdBy"
                    rows="3"
                    disabled
                    max-rows="6"
                  >
                  </b-form-input>
                </b-form-group>
              </div>
              <div class="col-lg-6 col-md-6 col-sm-6">
                <b-form-group
                  id="input-group-2"
                  label="Modificado en:"
                  label-for="input-2"
                >
                  <b-form-input
                    id="textarea"
                    v-model="principalSchema.lastModifiedDate"
                    rows="3"
                    disabled
                    max-rows="6"
                  >
                  </b-form-input>
                </b-form-group>
              </div>
            </div>
          </div>
        </div>
      </div>

      <div class="alert alert-light" role="alert">
        <div v-if="$route.query.Action == 'edit'">
          <b-button-group>
            <b-button
              variant="secundary"
              class="btn"
              @click="GoBack()"
              v-if="DataForm.backList"
            >
              <i class="bx bx-arrow-back"></i> Lista
            </b-button>
            <b-button variant="success" class="btn" @click="editSchema()">
              <i class="bx bx-save"></i> Actualizar
            </b-button>
          </b-button-group>
        </div>
        <div v-else>
          <b-button-group>
            <b-button variant="secundary" class="btn" @click="GoBack()">
              <i class="bx bx-arrow-back"></i> Lista
            </b-button>
            <b-button variant="success" size="lg" @click="post()">
              <i class="bx bx-save"></i> Crear
            </b-button>
          </b-button-group>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import mixpanel from "mixpanel-browser";
import Swal from "sweetalert2";
import { required } from "vuelidate/lib/validators";

var numbro = require("numbro");
var moment = require("moment");

export default {
  head() {
    return {
      title: `${this.DataForm.title}`,
    };
  },
  data() {
    return {
      file: "",
      filesTitle: [
        {
          key: "Acciones",
          label: "Archivos",
        },
      ],
      DataRows: [],
      Spinning: true,
      FormId: "",
      RowId: "",
      fields: ["Acción"],
      fieldsHorizon: [],
      DataForm: [],
      DataFormSection: [],
      DataFormSectionGrids: [],
      principalSchema: {},
      principalHorisonSchema: [],
      SchemaTable: [],
      requiredFields : [],
      form: {
        id: null,
        reference: null,
        commentary: null,
        date: "",
        typeRegisterId: "5e17b36a-fbbe-4c73-93ac-b112ee3ff08a",

        journaDetails: [
          {
            id: null,
            contactId: null,
            JournalId: null,
            ledgerAccountId: null,
            debit: 0.0,
            credit: 0.0,
            commentary: "",
          },
        ],
      },
      LedgerAccountes: [],
      Tdebit: 0.0,
      Tcredit: 0.0,
      show: true,
    };
  },
  validations: {
    form: {
      reference: {
        required,
      },
      date: {
        required,
      },
    },
  },
  watch: {
    "$route.query.Form"() {
      this.FormId = "";
      this.RowId = "";
      this.DataForm = [];
      this.files = [];
      this.requiredFields = [];
      this.DataFormSection = [];
      this.DataFormSectionGrids = [];
      this.principalSchema = {};
      this.SchemaTable = [];
      this.FormId = this.$route.query.Form;

      this.GetFormRows();
    },
  },
  middleware: "authentication",
  async mounted() {
    
    this.GetFormRows();
  },
  methods: {
    getRequiredFields() {
      
    for (const item of this.DataFormSection ) {
 
      for (const field of item.fields) {
      
        if (field.isRequired) { 
          this.requiredFields.push(field);
      }
     
    
    }

  }
},
validerrequiredFieldsfiled() {
    let validate = true;
 
    this.requiredFields.forEach((item) => {
       //hacer un trim eliminar espacios en blanco en this.principalSchema[item.field] 


      if (this.principalSchema[item.field] === undefined) {
        this.$toast.error(
          `El campo ${item.label} es requerido`,
          "Notificación",
          this.izitoastConfig
        );
        validate = false;
       
      }
    
       if (this.principalSchema[item.field] === null) {
         
        this.$toast.error(
          `El campo ${item.label} es requerido`,
          "Notificación",
          this.izitoastConfig
        );

        validate = false;
      }
      //validar si no tiene espacio en blanco
      if (this.principalSchema[item.field] === "") {
        this.$toast.error(
          `El campo ${item.label} no puede estar vacio`,
          "Notificación",
          this.izitoastConfig
        );
        validate = false;
      }
    });
    return validate;
  },
   
    GetFile() {
      let url = `FileManager/GetByReference?reference=${this.RowId}`;
      this.$axios
        .get(url)
        .then((response) => {
          this.files = response.data.data;
        })
        .catch((error) => {
       
        });
    
  },
    confirmCancellation(id) {
      let url = "";
      url = `FileManager/Delete/${id}`;
      Swal.fire({
        title: "estas seguro?",
        text: "esta seguro que quiere remover esta fila",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Si , Remuévela!",
        cancelButtonText: "Cancelar",
      }).then((result) => {
        if (result.isConfirmed) {
          this.$axios
            .delete(url)
            .then((response) => {
              this.GetFile();
              Swal.fire("Removido!", "El registro esta removido.", "success");
            })
            .catch((error) => alert(error));
        }
      });
    },
    // handleFileUpload() {
    //   this.file = this.$refs.file.files[0];
    // },
    async removeRow(index) {
     
      index.isActive = false;
      this.GetTotal();
      //this.form.journaDetails.splice(index, 1);
    },
    async GetTotal() {
      var Total = numbro(0);
      this.form.journaDetails.forEach((e) => {
        if (e.isActive) Total.add(e.debit);
      });
      this.Tdebit = Total.formatCurrency({
        thousandSeparated: true,
        mantissa: 2,
        negative: "parenthesis",
      });
      var TotalC = numbro(0);
      this.form.journaDetails.forEach((e) => {
        if (e.isActive) TotalC.add(e.credit);
      });
      this.Tcredit = TotalC.formatCurrency({
        thousandSeparated: true,
        mantissa: 2,
        negative: "parenthesis",
      });
    },
    async addRow() {
      let newRow = {
        id: null,
        contactId: null,
        JournalId: null,
        ledgerAccountId: null,
        debit: 0.0,
        credit: 0.0,
        commentary: "",
        isActive: true,
      };
      this.form.journaDetails.push(newRow);
    },
    async ValidaForm() {
      let validate = true;
      if (
        this.DataForm.transactionsType === 100 &&
        (this.Tcredit == 0 || this.Tdebit == 0)
      ) {
        this.$toast.error(
          `el debito y el credito no puede ser 0`,
          "Notificación",
          this.izitoastConfig
        );
        validate = false;
      }
      if (
        this.DataForm.transactionsType === 100 &&
        this.Tcredit !== this.Tdebit
      ) {
        this.$toast.error(
          `el debito y el credito no son iguales`,
          "Notificación",
          this.izitoastConfig
        );
        validate = false;
      }

      this.form.journaDetails.forEach((item) => {
        if (
          item.ledgerAccountId === null &&
          this.DataForm.transactionsType === 100
        ) {
          this.$toast.error(
            `Faltan por seleccionar cuentas contables`,
            "Notificación",
            this.izitoastConfig
          );
          validate = false;
        }
      });

      return validate;
    },
    GetFilterDataOnlyshowForm(fields) {
      let results = fields.filter((rows) => rows.showForm == 1);
      return results;
    },
    GetValueFormElement(formElemen) {
      this.principalSchema = formElemen;
    },

    GetFormRows() {
      this.FormId = this.$route.query.Form;
      var url = `Form/GetById?Id=${this.$route.query.Form}`;
      this.DataForm = [];
      this.DataFormSection = [];
      this.DataFormSectionGrids = [];

      this.$axios
        .get(url)
        .then(async (response) => {
          this.DataForm = response.data.data;

          this.GetFilds();
          await this.getLeaderAccount();
          if (this.$route.query.Action === "edit") {
            this.RowId = this.$route.query.Id;
            this.GetFildsData();
            this.GetFile();
          }
        })
        .catch((error) => {
          //this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
        });
    },
    GetFilds: function () {
      this.$axios
        .get(`Formfields/GetSectionWithFildsByFormID/${this.FormId}`)
        .then((response) => {
          this.DataFormSection = response.data.data;
          this.fieldsHorizon = response.data.data[0].fields;
          this.Spinning = false;
          this.getRequiredFields();
        })
        .catch((error) => {
          //this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
        });
    },

    GetFildsData() {
      var url = `${this.DataForm.controller}/GetById?Id=${this.RowId}`;
      this.$axios
        .get(url)
        .then(async (response) => {
          this.form = response.data.data;

          await this.GetTotal();
        })
        .catch((error) => {
          console.error(error);
          //this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
        });
    },

    async getLeaderAccount() {
      let url = `LedgerAccount/GetAll`;
      let result = null;
      this.$axios
        .get(url)
        .then((response) => {
          result = response;
          this.LedgerAccountes = result.data.data;
        })
        .catch((error) => {
          result = error;
        });
    },
    GoBack() {
      if (this.DataForm.backList) {
        this.$router.push({ path: `/ExpressForm/Index?Form=${this.FormId}` });
      }
    },

    editSchema() {
      if (this.DataForm.edit == true) {
        this.put();
      } else {
        //this.$toast.info("La opcion editar esta deshabilitada");
      }
    },
    saveSchema() {
      if (this.RowId.length < 1) {
        if (this.DataForm.create == true) {
          if (this.validerrequiredFieldsfiled()) {
            this.post();
          }
        
        } else {
          //this.$toast.info("La opcion crear esta deshabilitada");
        }
      }
    },

    post() {
      if (this.validerrequiredFieldsfiled()) {
      this.$v.$touch();

      if (
        this.$v.$invalid &&
        this.ValidaForm() &&
        this.DataForm.transactionsType === 100
      ) {
        this.$toast.error(
          "Por favor complete el formulario correctamente.",
          "ERROR",
          this.izitoastConfig
        );
      } else {
        let url = `${this.DataForm.controller}/Create`;
        let result = null;
        this.$axios
          .post(url, this.principalSchema)
          .then((response) => {
            if (response.data.succeeded) {
              this.$toast.success(
                "El Registro ha sido creado correctamente.",
                "ÉXITO"
              );
              this.GoBack();
            } else {
              this.$toast.info(
                `${response.data.friendlyMessage}`,
                "Informaciòn",
                this.izitoastConfig
              );
            }
          })
          .catch((error) => {
            result = error;
            
            mixpanel.track("FromDynamicExpress/Post" + result);
            this.$toast.error(`${result}`, "Informaciòn", this.izitoastConfig);
          });
      }
    }
    },
    put() {
      this.$v.$touch();

      if (
        this.$v.$invalid &&
        this.ValidaForm() &&
        this.DataForm.transactionsType === 100
      ) {
        this.$toast.error(
          "Por favor complete el formulario correctamente.",
          "ERROR",
          this.izitoastConfig
        );
      } else {
        this.$axios
          .put(`${this.DataForm.controller}/Update`, this.principalSchema)
          .then((response) => {
            if (response.data.succeeded) {
              this.$toast.success(
                "El Registro ha sido actualizado correctamente..",
                "ÉXITO"
              );
              this.GoBack();
            } else {
              this.$toast.info(
                `${response.data.friendlyMessage}`,
                "Informaciòn",
                this.izitoastConfig
              );
            }
          })
          .catch((error) => {
            reject(error);
            mixpanel.track("FromDynamicExpress/Pust" + error);
            // this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
          });
      }
    },
  },
};
</script>

<style>
/* Red border */
hr.new1 {
  border-top: 1px solid blue;
}

.text-size-required {
  font-size: 12px;
}
</style>
