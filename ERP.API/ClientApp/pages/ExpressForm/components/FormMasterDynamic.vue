<template>
  <div>
    <h4>{{ DataForm.title }}</h4>

    <div class="row">

      <div class="col-lg-12">
        <div class="card">
          <div class="card-body">
            <div
              v-for="(SectionRow, SectionIndex) in DataFormSection"
              :key="SectionIndex"
            >
              <div class="row ml-0 mb-3">
                <div class="col-lg-12 col-md-12 col-sm-12">
                  <h4>{{ SectionRow.name }}</h4>
                  <hr class="new1"/>
                </div>
              </div>

              <div class="row">
                <div
                  class="col-4"
                  v-for="(fieldsRow, fieldIndex) in GetFilterDataOnlyshowForm(
                    SectionRow.fields
                  )"
                  :key="fieldIndex"
                >

                  <DynamicElementGrid
                    @CustomChange="GetValueFormElement"
                    :FieldsData="principalSchema"
                    :item="fieldsRow"
                    :labelShow="true"
                  ></DynamicElementGrid>
                </div>
              </div>
            </div>

            <table class="w-100">
              <thead>
              <tr>
                <th>
                  <template v-if="list.length < 1">
                    <b-button variant="primary" @click="addRow()">
                      <span> <i class="fas fa-plus"></i> </span>
                    </b-button>
                  </template>
                  Concepto
                </th>

                <th>Cantidad</th>
                <th class=" bg-warning">P.Impuesto</th>
                <th class=" bg-warning">I.Total</th>
                <th>Precio</th>
                <th>Total</th>
              </tr>
              </thead>
              <tbody>

              <tr v-for="(item, index) in list" :key="index">
                <td>
                  <b-form-group>
                    <vueselect
                      style="width: 350px"
                      :options="conceptSelectList"
                      v-model="item.referenceId"
                      :reduce="(row) => row.id"
                      label="description"
                      placeholder="Escriba 3 o mas digitos para buscar"
                      @search="onSearch"
                      @input="setSelected(item, index)"
                      size="sm"
                    >
                      <template v-slot:option="option">
                          <span>

                            {{ option.description }} <strong> Ref:</strong>
                            {{ option.reference }}
                            P.V ${{
                              SetTotal(option.priceSale)

                            }}
                            P.I  ${{

                              SetTotal(option.priceWithTax)
                            }}
                          </span>
                      </template>
                    </vueselect>
                  </b-form-group>
                </td>

                <td>
                  <b-form-group>
                    <b-form-input
                      v-model="item.amount"
                      class="mb-2"
                      type="number"
                      @input="calculateLineTotal(item)"

                      size="sm"
                    >
                    </b-form-input>
                  </b-form-group>
                </td>
                <td class=" bg-warning">
                  <b-form-group>
                    <b-form-input
                      v-model="item.priceWithTax"
                      class="mb-2"
                      type="number"

                      @change="calculateLineTotalWithTax(item)"
                      size="sm"
                    >
                    </b-form-input>
                  </b-form-group>
                </td>
                <td class=" bg-warning">
                  <b-form-group>
                    <b-form-input
                      v-model="item.totalTax"
                      class="mb-2"
                      type="number"
                      disabled
                      size="sm"
                    ></b-form-input>
                  </b-form-group>
                </td>
                <td>
                  <b-form-group>
                    <b-form-input
                      v-model="item.price"
                      class="mb-2"
                      type="number"
                      @change="calculateLineTotal(item)"
                      size="sm"
                    >
                    </b-form-input>
                  </b-form-group>
                </td>

                <td>
                  <b-form-group>
                    <b-form-input
                      v-model="item.total"
                      class="mb-2"
                      type="number"
                      disabled
                      size="sm"
                    ></b-form-input>
                  </b-form-group>
                </td>
                <td>
                  <b-button-group class="mt-4 mt-md-0">
                    <b-button
                      size="sm"
                      variant="danger"
                      @click="removeRow(index)"
                    >
                      <i class="fas fa-trash"></i>
                    </b-button>
                    <b-button size="sm" variant="info" @click="addRow()">
                      <i class="fas fa-plus"></i>
                    </b-button>
                  </b-button-group>
                </td>
              </tr>
              </tbody>
            </table>

            <div class="row ">
              <div class="col-lg-9" v-if="false">
                <div
                  v-for="(SectionRow, SectionIndex) in DataFormSectionTax"
                  :key="SectionIndex"
                >
                  <div class="row ml-0 mb-3">
                    <div class="col-lg-12 col-md-12 col-sm-12">
                      <h4>{{ SectionRow.name }}</h4>
                      <hr class="new1"/>
                    </div>
                  </div>

                  <div class="row">
                    <div
                      class="col-4"
                      v-for="(fieldsRow, fieldIndex) in GetFilterDataOnlyshowForm(
                    SectionRow.fields
                  )"
                      :key="fieldIndex"
                    >

                      <DynamicElementGrid
                        @CustomChange="GetValueFormElement"
                        :FieldsData="principalSchema"
                        :item="fieldsRow"
                        :labelShow="true"
                      ></DynamicElementGrid>
                    </div>
                  </div>
                </div>

              </div>
              <div class="col-lg-2 offset-lg-10">
                <table>
                  <tr>
                    <th>SubTotal</th>
                    <td>
                      {{ invoice_subtotal }}
                    </td>

                  </tr>
                  <tr>

                    <th>Total</th>
                    <td>
                      {{ invoice_total }}

                    </td>
                  </tr>
                  <tr class=" bg-warning">

                    <th>I.SubTotal</th>
                    <td class=" bg-white">
                      {{ invoice_subtotalTax }}
                    </td>
                  </tr>
                  <tr class=" bg-warning">

                    <th>I.Impuestos</th>
                    <td class=" bg-white">
                      {{ this.GetTax()}}

                    </td>
                  </tr>

                  <tr class=" bg-warning">

                    <th>I.Total</th>
                    <td class=" bg-white">
                      {{ this.GetTotaltax() }}

                    </td>
                  </tr>

                </table>

              </div>


            </div>
            <div class="row justify-content-end w-100 gx-2">
              <div class="col-lg-3 offset-lg-9" v-if="$route.query.Action === 'edit'">
                <b-button-group class="mt-4 mt-md-0">
                  <b-button variant="secundary" class="btn" size="sm" @click="GoBack()">
                    <i class="bx bx-arrow-back"></i> Lista
                  </b-button>
                  <b-button variant="success" class="btn" size="sm" @click="editSchema()">
                    <i class="bx bx-save"></i> Guardar
                  </b-button>
                  <b-button
                    variant="primary"
                    title="Imprimir"
                    @click="editSchemaPrint()" size="sm"
                  >
                    <i class="uil uil-print font-size-18"></i> Guardar Imprimir
                  </b-button>
                </b-button-group>
              </div>
              <div class="col-3 p-2" v-else>
                <b-button-group class="mt-4 mt-md-0">
                  <b-button variant="secundary" class="btn" size="sm" @click="GoBack()">
                    <i class="bx bx-arrow-back"></i> Lista
                  </b-button>
                  <b-button variant="success" size="sm" @click="saveSchema()">
                    <i class="bx bx-save"></i> Guardar
                  </b-button>
                  <b-button
                    variant="primary"
                    title="Imprimir"
                    @click="saveSchemaPrint()" size="sm"
                  >
                    <i class="uil uil-print font-size-18"></i> Guardar Imprimir
                  </b-button>
                </b-button-group>
              </div>
            </div>
            <div class="row ml-0 mb-3">
              <div class="col-lg-12 col-md-12 col-sm-12">
                <hr class="new1"/>
                <b-form-group id="input-group-2" label-for="input-2">
                  <h4 class="card-title">Comentario</h4>
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
                <h4>Auditoría</h4>
                <hr class="new1"/>
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
    </div>
  </div>
</template>

<script>
var numbro = require("numbro");
var moment = require("moment");
export default {
  head() {
    return {
      title: `${this.DataForm.title} | ERP`,
    };
  },
  data() {
    return {
      limit: 10,
      search: "",

      offset: 0,
      FormId: "",
      DataForm: [],
      DataFormSection: [],
      DataFormTax: [],
      DataFormSectionTax: [],
      fields: [],
      fieldsTax: [],
      principalSchema: {
        contactId: null,
        code: null,
        date: null,
        reference: null,
        globalDiscount: 0.0,
        globalTotal: 0.0,
        globalTotalTax: 0.0,
        transactionsType: 1,
        transactionsDetails: null,
        numerationId: null,
        commentary: "",
        taxesId: "69a423e6-1b00-4873-9003-e83d9ff13bda"
      },
      schema: {
        id: null,
        transactionsId: null,
        referenceId: null,
        description: null,
        amount: 1,
        price: 0,
        discount: 0,
        total: 0,
        tax: 0,
      },
      conceptSelectList: [],
      rows: [],
      invoice_subtotal: 0,
      invoice_total: 0,
      invoice_totalTax: 0,
      invoice_subtotalTax: 0,
      invoice_tax: 0,
      list: [
        {
          id: null,
          concept: null,
          transactionsId: null,
          referenceId: null,
          description: null,
          amount: 1,
          price: 0,
          priceWithTax: 0,
          discount: 0,
          total: 0,
          totalTax: 0,
          tax: 0,
        },
      ],
    };
  },


  created() {
    this.FormId = this.$route.query.Form;
    this.GetFormRows();
    //this.GetFormRowsTax();

    if (this.$route.query.Action === "edit") {
      this.getTransactionsDetails();
    }
  },
  methods: {
    onSearch(query) {
      this.search = query;
      if (query.length >= 3) {
        let url = `Concept/GetFilter?PageNumber=${this.offset}&PageSize=${this.limit}&Search=${this.search}`;

        this.$axios
          .get(`${url}`)
          .then((response) => {
            var Items = response.data.data.data;

            if (this.conceptSelectList.length === 0) {
              this.conceptSelectList = Items;
            } else {
              Items.forEach(item => {
                // Check if the item is not already in conceptSelectList
                if (!this.conceptSelectList.some(existingItem => existingItem.id === item.id)) {
                  this.conceptSelectList.push(item); // Add the item to conceptSelectList
                }
              });
            }


          })
          .catch((error) => {

          });
      }
    },
    SetTotal(globalTotal) {
      return numbro(globalTotal).format("0,0.00");
    },
    GetTotaltax() {

      var tax = this.invoice_totalTax * 0.18;
      var Taxcal = tax + this.invoice_totalTax;
      return numbro(parseFloat(this.invoice_totalTax) + parseFloat(Taxcal)).format("0,0.00");

    },
    GetTax() {

      var tax = this.invoice_totalTax * 0.18;
      var Taxcal = tax + this.invoice_totalTax;
      return numbro(  parseFloat(Taxcal)).format("0,0.00");

    },

    getDate() {
      const date = new Date();
      let day = date.getDate();
      let month = date.getMonth() + 1;
      let year = date.getFullYear();
      let currentDate = `${day}/${month}/${year}`;
      this.principalSchema.date = currentDate;
    },
    GetValueFormElement(formElemen) {
      this.principalSchema = formElemen;
    },

    GetLitValue(filds, Value) {
      this.principalSchema[filds] = Value;
    },
    GetFilterDataOnlyshowForm(fields) {
      let results = fields.filter((rows) => rows.showForm == 1);
      return results;
    },
    setSelected(data, idx) {
      var obj = this.list.find((element, index) => index === idx);
      let row = this.conceptSelectList.find(
        (element) => element.id == obj.referenceId
      );

      obj.referenceId = row.id;
      obj.price = row.priceSale;
      obj.priceWithTax = row.priceWithTax;
      this.calculateLineTotal(obj);
      this.calculateLineTotalWithTax(obj);
    },
    GetFormRows() {
      var url = `Form/GetById?Id=${this.FormId}`;
      this.DataForm = [];
      this.DataFormSection = [];
      this.$axios
        .get(url)
        .then((response) => {
          this.DataForm = response.data.data;


          this.GetFilds();
        })
        .catch((error) => {
          this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
        });
    },
    GetFormRowsTax() {
      var url = `Form/GetById?Id=24aa3aac-d793-4ca1-b7c9-59cc279e0354`;
      this.DataFormTax = [];
      this.DataFormSectionTax = [];
      this.$axios
        .get(url)
        .then((response) => {
          this.DataFormTax = response.data.data;

          this.GetFildsTax();
        })
        .catch((error) => {
          this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
        });
    },

    printForm(id) {
      this.$router.push({
        path: `/ExpressForm/Ticket?Action=print&Form=${this.FormId}&Id=${id}`,
      });
    },
    GetFilds() {
      this.$axios
        .get(`Formfields/GetSectionWithFildsByFormID/${this.FormId}`)
        .then((response) => {
          this.DataFormSection = response.data.data;
        })
        .catch((error) => {
          this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
        });
    },
    GetFildsTax() {
      this.$axios
        .get(`Formfields/GetSectionWithFildsByFormID/24aa3aac-d793-4ca1-b7c9-59cc279e0354`)
        .then((response) => {
          this.DataFormSectionTax = response.data.data;
        })
        .catch((error) => {
          this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
        });
    },

    calculateTotalTax() {
      var subtotal, total;
      subtotal = this.list.reduce(function (sum, product) {
        var lineTotal = parseFloat(product.totalTax);
        if (!isNaN(lineTotal)) {
          return sum + lineTotal;
        }
      }, 0);

      this.invoice_subtotalTax = subtotal.toFixed(2);

      total = subtotal * (this.invoice_tax / 100) + subtotal;
      total = parseFloat(total);
      if (!isNaN(total)) {
        this.invoice_totalTax = total.toFixed(2);
        this.principalSchema.globalTotalTax = this.invoice_totalTax;
      } else {
        this.invoice_totalTax = "0.00";
        this.principalSchema.globalTotalTax = this.invoice_totalTax;
      }
    },
    calculateTotal() {
      var subtotal, total;
      subtotal = this.list.reduce(function (sum, product) {
        var lineTotal = parseFloat(product.total);
        if (!isNaN(lineTotal)) {
          return sum + lineTotal;
        }
      }, 0);

      this.invoice_subtotal = subtotal.toFixed(2);

      total = subtotal * (this.invoice_tax / 100) + subtotal;
      total = parseFloat(total);
      if (!isNaN(total)) {
        this.invoice_total = total.toFixed(2);
        this.principalSchema.globalTotal = this.invoice_total;
      } else {
        this.invoice_total = "0.00";
        this.principalSchema.globalTotal = this.invoice_total;
      }
    },
    calculateLineTotal(invoiceProduct) {
      var total =
        parseFloat(invoiceProduct.price) * parseFloat(invoiceProduct.amount);
      if (!isNaN(total)) {
        invoiceProduct.total = total.toFixed(2);
      }
      this.calculateTotal();
      this.calculateLineTotalWithTax(invoiceProduct);
    },
    calculateLineTotalWithTax(invoiceProduct) {
      var total =
        parseFloat(invoiceProduct.priceWithTax) * parseFloat(invoiceProduct.amount);
      if (!isNaN(total)) {
        invoiceProduct.totalTax = total.toFixed(2);
      }
      this.calculateTotalTax();
    },

    GoBack() {
      this.$router.push({path: `/ExpressForm/Index?Form=${this.FormId}`});
    },

    removeRow(index) {
      this.list.splice(index, 1);
    },
    addRow() {
      this.list.push({
        id: null,
        transactionsId: null,
        referenceId: null,
        description: null,
        amount: 1,
        price: 0,
        discount: 0,
        total: 0,
        tax: 0,
        code: null,
        concept: null,
      });
    },
    FormatDate(date) {
      return moment(date).lang("es").format("DD/MM/YYYY");
    },
    editSchema() {
      if (this.principalSchema.contactId != null) {
        if (this.invoice_total > 0) {
          this.put(this.principalSchema);
        } else {
          this.$toast.error(`Total no puede ser 0`, "ERROR", this.izitoastConfig);
        }
      } else {
        this.$toast.error(`Contacto es requerido`, "ERROR", this.izitoastConfig);
      }
    },
    editSchemaPrint() {
      if (this.principalSchema.contactId != null) {
        if (this.invoice_total > 0) {

          this.putPrint(this.principalSchema);
        } else {
          this.$toast.error(`Total no puede ser 0`, "ERROR", this.izitoastConfig);
        }
      } else {
        this.$toast.error(`Contacto es requerido`, "ERROR", this.izitoastConfig);
      }
    },
    saveSchema() {
      if (this.principalSchema.contactId != null) {
        if (this.invoice_total > 0) {
          this.principalSchema.transactionsDetails = this.list;

          this.post(this.principalSchema);
        } else {
          this.$toast.error(`Total no puede ser 0`, "ERROR", this.izitoastConfig);
        }
      } else {
        this.$toast.error(`Contacto es requerido`, "ERROR", this.izitoastConfig);
      }
    },
    saveSchemaPrint() {
      if (this.principalSchema.contactId != null) {
        this.principalSchema.transactionsDetails = this.list;
        if (this.invoice_total > 0) {
          this.postPrint(this.principalSchema);
        } else {
          this.$toast.error(`Total no puede ser 0`, "ERROR", this.izitoastConfig);
        }
      } else {
        this.$toast.error(`Contacto es requerido`, "ERROR", this.izitoastConfig);
      }
    },
    async getTransactionsDetails() {
      let url = `Transaction/GetById?id=${this.$route.query.Id}`;
      this.$axios
        .get(url)
        .then((response) => {
          this.principalSchema = response.data.data;
          this.list = response.data.data.transactionsDetails;
          this.list.forEach((elemento) => {
            this.conceptSelectList.push(elemento.concept);
          });
          this.calculateTotal();
          this.calculateTotalTax();
        })
        .catch((error) => {
          //this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
        });
    },
    ValideForm() {
      let TotalList = this.list.length;

      if (TotalList <= 0) {
        return false;
      }

      return true;
    },
    postPrint(data) {
      data.transactionsType = this.DataForm.transactionsType;
      data.formId = this.FormId;

      let url = `Transaction/Create`;
      let result = null;
      let frmResult = this.ValideForm();
      this.$axios
        .post(url, data)
        .then((response) => {
          result = response;

          this.$toast.success(
            "El Registro ha sido creado correctamente.",
            "ÉXITO"
          );

          this.printForm(result.data.data.id);
        })
        .catch((error) => {
          result = error;
          //  this.$toast.error(`${result}`, "ERROR", this.izitoastConfig);
        });
    },
    post(data) {
      data.transactionsType = this.DataForm.transactionsType;
      data.formId = this.FormId;

      let url = `Transaction/Create`;
      let result = null;
      let frmResult = this.ValideForm();

      this.$axios
        .post(url, data)
        .then((response) => {
          result = response;

          this.$toast.success(
            "El Registro ha sido creado correctamente.",
            "ÉXITO"
          );
          this.GoBack();
        })
        .catch((error) => {
          result = error;
          // this.$toast.error(`${result}`, "ERROR", this.izitoastConfig);
        });
    },
    put(data) {
      data.transactionsType = this.DataForm.transactionsType;
      data.formId = this.FormId;
      let frmResult = this.ValideForm();

      this.$axios
        .put("Transaction/Update", data)
        .then((response) => {
          this.$toast.success(
            "El Registro ha sido actualizado correctamente.",
            "EXITO"
          );

          this.GoBack();
        })
        .catch((error) => {

          //  this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
        });
    },
    putPrint(data) {
      data.transactionsType = this.DataForm.transactionsType;
      data.formId = this.FormId;
      let frmResult = this.ValideForm();
      this.$axios
        .put("Transaction/Update", data)
        .then((response) => {
          this.$toast.success(
            "El Registro ha sido actualizado correctamente.",
            "EXITO"
          );

          this.printForm(data.id);
        })
        .catch((error) => {
          reject(error);
          //   this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
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
                instance.hide({transitionOut: "fadeOut"}, toast, "button");
                this.$axios
                  .delete(`Transaction/Delete/?id=${id}`)
                  .then((response) => {
                    alert(
                      "EXITO: El Registro ha sido eliminado correctamente."
                    );
                    location.reload();
                  })
                  .catch((error) => {
                    //reject(error);
                    ///this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
                  });
              },
              true,
            ],
            [
              "<button>NO</button>",
              function (instance, toast) {
                instance.hide({transitionOut: "fadeOut"}, toast, "button");
              },
            ],
          ],
        }
      );
    },
    clearForm() {
      (this.invoice_subtotal = 0),
        (this.invoice_total = 0),
        (this.principalSchema = {
          contactId: null,
          code: null,
          date: null,
          reference: null,
          paymentMethodId: null,
          globalDiscount: 0.0,
          globalTotal: 0.0,
          transactionsType: 1,
          transactionsDetails: null,
          commentary: "",
        });
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
