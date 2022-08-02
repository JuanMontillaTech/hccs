<template>
    <div>
        <h4> {{ DataForm.title }}</h4>
        {{principalSchema}}
        <div class="row">
            <div class="col-lg-12">
                <div class="card">
                    <div class="card-body">
 <div class="col-lg-12">
        <div class="card">
          <div class="card-body">
            <div class="row">
              <div v-for="(item, ind in fields" :key="ind" class="col-6" v-if="item.showForm ==1">
                <div v-if="item.columnIndex == 1">
                  <div class="row">
                    <div class="col-12">
                 
                      <b-form-group   v-if="item.type == 0">
                        <h4 class="card-title"> {{item.label}}</h4>
                        <b-form-input v-model="principalSchema[item.field]" size="sm" trim>
                        </b-form-input>
                      </b-form-group>
                      <b-form-group v-if="item.type == 5">
                             <h4 class="card-title"> {{item.label}}</h4>
                        <b-form-textarea   v-model="principalSchema[item.field]" rows="3"
                          max-rows="6"></b-form-textarea>
                      </b-form-group>
                      <b-form-group  v-if="item.type == 1">
                             <h4 class="card-title"> {{item.label}}</h4>
                        <vSelect :field="item" @CustomChange="GetLitValue" :select="principalSchema[item.field]">
                        </vSelect>
                      </b-form-group>

                      <b-form-group v-if="item.type == 3" > 
                           <h4 class="card-title"> {{item.label}}</h4>
                    <input type="checkbox" id="checkbox" v-model="principalSchema[item.field]"> 
                      </b-form-group>
                      <b-form-group  v-if="item.type == 4">
                <h4 class="card-title"> {{item.label}}</h4>
                <b-form-datepicker  v-model="principalSchema[item.field]"  locale="es" :disabled="$route.query.Action == 'show'"
                  class="mb-2"></b-form-datepicker>
                 
              </b-form-group>

                    </div>
                  </div>
                </div>
                <div v-if="item.columnIndex == 2">
                  <div class="row">
                     <div class="col-12">
                 
                      <b-form-group   v-if="item.type == 0">
                        <h4 class="card-title"> {{item.label}}</h4>
                        <b-form-input v-model="principalSchema[item.field]" size="sm" trim>
                        </b-form-input>
                      </b-form-group>
                      <b-form-group v-if="item.type == 5">
                             <h4 class="card-title"> {{item.label}}</h4>
                        <b-form-textarea   v-model="principalSchema[item.field]" rows="3"
                          max-rows="6"></b-form-textarea>
                      </b-form-group>
                      <b-form-group  v-if="item.type == 1">
                             <h4 class="card-title"> {{item.label}}</h4>
                        <vSelect :field="item" @CustomChange="GetLitValue" :select="principalSchema[item.field]">
                        </vSelect>
                      </b-form-group>

                      <b-form-group v-if="item.type == 3"  > 
                           <h4 class="card-title"> {{item.label}}</h4>
                    <input type="checkbox" id="checkbox" v-model="principalSchema[item.field]"> 
                      </b-form-group>
                      <b-form-group  v-if="item.type == 4">
                <h4 class="card-title"> {{item.label}}</h4>
                <b-form-datepicker  v-model="principalSchema[item.field]"  locale="es" :disabled="$route.query.Action == 'show'"
                  class="mb-2"></b-form-datepicker>
                 
              </b-form-group>

                    </div>
                  </div>
                </div>
              </div>

            </div>


            <div class="row justify-content-end w-100 gx-2">
              <div class="col-3 p-2" v-if="$route.query.Action == 'edit'">
                <b-button-group class="mt-4 mt-md-0">
                  <b-button variant="secundary" class="btn" @click="GoBack()">
                    <i class="bx bx-arrow-back"></i> Regresar
                  </b-button>
                  <b-button variant="success" class="btn" @click="editSchema()">
                    <i class="bx bx-save"></i> Guardar
                  </b-button>
                </b-button-group>
              </div>
              <div class="col-3 p-2" v-else>
                <b-button-group class="mt-4 mt-md-0">
                  <b-button variant="secundary" class="btn" @click="GoBack()">
                    <i class="bx bx-arrow-back"></i> Regresar
                  </b-button>
                  <b-button variant="success" size="lg" @click="saveSchema()">
                    <i class="bx bx-save"></i> Guardar
                  </b-button>
                </b-button-group>
              </div>
            </div>
            <div class="row ml-0 mb-3">
              <div class="col-lg-12 col-md-12 col-sm-12">
                <hr class="new1" />
                <b-form-group id="input-group-2" label-for="input-2">
                  <h4 class="card-title">Comentario</h4>
                  <b-form-textarea id="textarea" v-model="principalSchema.commentary" rows="3" max-rows="6">
                  </b-form-textarea>
                </b-form-group>
              </div>
            </div>
            <div class="row ml-0 mb-3">
              <div class="col-lg-12 col-md-12 col-sm-12">
                <h4>Auditoría</h4>
                <hr class="new1" />
              </div>
            </div>
            <div class="row ml-0 mb-3">
              <div class="col-lg-6 col-md-6 col-sm-6">
                <b-form-group id="input-group-2" label="Creado Por :" label-for="input-2">
                  <b-form-input id="textarea" v-model="principalSchema.createdBy" rows="3" disabled max-rows="6">
                  </b-form-input>
                </b-form-group>
              </div>
              <div class="col-lg-6 col-md-6 col-sm-6">
                <b-form-group id="input-group-2" label="Creado en :" label-for="input-2">
                  <b-form-input id="textarea" v-model="principalSchema.createdDate" rows="3" disabled max-rows="6">
                  </b-form-input>
                </b-form-group>
              </div>
            </div>
            <div class="row ml-0 mb-3">
              <div class="col-lg-6 col-md-6 col-sm-6">
                <b-form-group id="input-group-2" label="Modificado Por:" label-for="input-2">
                  <b-form-input id="textarea" v-model="principalSchema.createdBy" rows="3" disabled max-rows="6">
                  </b-form-input>
                </b-form-group>
              </div>
              <div class="col-lg-6 col-md-6 col-sm-6">
                <b-form-group id="input-group-2" label="Modificado en:" label-for="input-2">
                  <b-form-input id="textarea" v-model="principalSchema.lastModifiedDate" rows="3" disabled max-rows="6">
                  </b-form-input>
                </b-form-group>
              </div>
            </div>
          </div>
        </div>
      </div>
                  

                        <table class="table striped table-border">
                            <thead>
                                <tr>
                                    <th>
                                        <template v-if="list.length < 1">
                                            <b-button variant="primary" @click="addRow()"
                                                :disabled="$route.query.Action == 'show'">
                                                <span> <i class="fas fa-plus"></i> </span>
                                            </b-button>
                                        </template>
                                        Concepto
                                    </th>
                                    <th>Descripción</th>
                                    <th>Cantidad</th>
                                    <th>Precio</th>
                                    <!-- <th>Descuento %</th> -->
                                    <th>Neto</th>
                                    <!-- <th>Impuesto %</th> -->
                                    <th>Total</th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr v-for="(item, index) in list" :key="index">
                                    <td>
                                        <b-form-group>
                                            <vueselect style="width: 350px" :options="conceptSelectList"
                                                v-model="item.referenceId" :reduce="(row) => row.id" label="reference"
                                                :disabled="$route.query.Action == 'show'"
                                                @input="setSelected(item, index)" size="sm"></vueselect>
                                        </b-form-group>
                                    </td>
                                    <td>
                                        <b-form-group>
                                            <b-form-input v-model="item.description" class="mb-2"
                                                :disabled="$route.query.Action == 'show'" size="sm"></b-form-input>
                                        </b-form-group>
                                    </td>
                                    <td>
                                        <b-form-group>
                                            <b-form-input v-model="item.amount" class="mb-2" type="number"
                                                :disabled="$route.query.Action == 'show'"
                                                @change="calculateLineTotal(item)" size="sm">
                                            </b-form-input>
                                        </b-form-group>
                                    </td>
                                    <td>
                                        <b-form-group>
                                            <b-form-input v-model="item.price" class="mb-2" type="number"
                                                :disabled="$route.query.Action == 'show'"
                                                @change="calculateLineTotal(item)" size="sm">
                                            </b-form-input>
                                        </b-form-group>
                                    </td>

                                    <td>
                                        <b-form-group>
                                            <b-form-input v-model="item.total" class="mb-2" type="number"
                                                :disabled="$route.query.Action == 'show'" size="sm"></b-form-input>
                                        </b-form-group>
                                    </td>

                                    <td>
                                        <b-form-group>
                                            <b-form-input v-model="item.total" class="mb-2" type="number" disabled
                                                size="sm"></b-form-input>
                                        </b-form-group>
                                    </td>
                                    <td>
                                        <b-button-group class="mt-4 mt-md-0">
                                            <b-button size="sm" variant="danger" @click="removeRow(index)"
                                                :disabled="$route.query.Action == 'show'" v-b-tooltip.hover>
                                                <i class="fas fa-trash"></i>
                                            </b-button>
                                            <b-button size="sm" variant="info" @click="addRow()"
                                                :disabled="$route.query.Action == 'show'">
                                                <i class="fas fa-plus"></i>
                                            </b-button>
                                        </b-button-group>
                                    </td>
                                </tr>
                            </tbody>
                        </table>

                        <div class="row ml-0 mb-3">
                            <div class="col-lg-3">
                                <b-form-group>
                                    <h4 class="card-title">SubTotal</h4>
                                    <b-form-input v-model="invoice_subtotal" disabled></b-form-input>
                                </b-form-group>
                            </div>
                            <div class="col-lg-3">
                                <b-form-group>
                                    <h4 class="card-title">Total</h4>
                                    <b-form-input v-model="invoice_total" disabled></b-form-input>
                                </b-form-group>
                            </div>
                            <div class="col-lg-3">
                                <b-form-group>
                                    <h4 class="card-title">Impuesto %</h4>
                                    <b-form-input v-model="invoice_tax" disabled></b-form-input>
                                </b-form-group>
                            </div>
                        </div>
                        <div class="row justify-content-end w-100 gx-2">
                            <div class="col-3 p-2" v-if="$route.query.Action == 'edit'">
                                <b-button-group class="mt-4 mt-md-0">
                                    <b-button variant="secundary" class="btn" @click="GoBack()">
                                        <i class="bx bx-arrow-back"></i> Regresar
                                    </b-button>
                                    <b-button variant="success" class="btn" @click="editSchema()">
                                        <i class="bx bx-save"></i> Guardar
                                    </b-button>
                                </b-button-group>
                            </div>
                            <div class="col-3 p-2" v-else>
                                <b-button-group class="mt-4 mt-md-0">
                                    <b-button variant="secundary" class="btn" @click="GoBack()">
                                        <i class="bx bx-arrow-back"></i> Regresar
                                    </b-button>
                                    <b-button variant="success" size="lg" @click="saveSchema()">
                                        <i class="bx bx-save"></i> Guardar
                                    </b-button>
                                </b-button-group>
                            </div>
                        </div>
                        <div class="row ml-0 mb-3">
                            <div class="col-lg-12 col-md-12 col-sm-12">
                                <hr class="new1" />
                                <b-form-group id="input-group-2" label-for="input-2">
                                    <h4 class="card-title">Comentario</h4>
                                    <b-form-textarea id="textarea" v-model="principalSchema.commentary" rows="3"
                                        max-rows="6">
                                    </b-form-textarea>
                                </b-form-group>
                            </div>
                        </div>
                        <div class="row ml-0 mb-3">
                            <div class="col-lg-12 col-md-12 col-sm-12">
                                <h4>Auditoría</h4>
                                <hr class="new1" />

                            </div>
                        </div>
                        <div class="row ml-0 mb-3">
                            <div class="col-lg-6 col-md-6 col-sm-6">
                                <b-form-group id="input-group-2" label="Creado Por :" label-for="input-2">
                                    <b-form-input id="textarea" v-model="principalSchema.createdBy" rows="3" disabled
                                        max-rows="6">
                                    </b-form-input>
                                </b-form-group>
                            </div>
                            <div class="col-lg-6 col-md-6 col-sm-6">
                                <b-form-group id="input-group-2" label="Creado en :" label-for="input-2">
                                    <b-form-input id="textarea" v-model="principalSchema.createdDate" rows="3" disabled
                                        max-rows="6">
                                    </b-form-input>
                                </b-form-group>
                            </div>
                        </div>
                        <div class="row ml-0 mb-3">
                            <div class="col-lg-6 col-md-6 col-sm-6">
                                <b-form-group id="input-group-2" label="Modificado Por:" label-for="input-2">
                                    <b-form-input id="textarea" v-model="principalSchema.createdBy" rows="3" disabled
                                        max-rows="6">
                                    </b-form-input>
                                </b-form-group>
                            </div>
                            <div class="col-lg-6 col-md-6 col-sm-6">
                                <b-form-group id="input-group-2" label="Modificado en:" label-for="input-2">
                                    <b-form-input id="textarea" v-model="principalSchema.lastModifiedDate" rows="3"
                                        disabled max-rows="6">
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
import { required } from "vuelidate/lib/validators";
export default {
    head() {
        return {
            title: `${this.DataForm.title} | ERP Cardenal Sancha`,
        };
    },
    data() {
        return {
            DataForm: {},
            fields: [],
            principalSchema: {
                contactId: null,
                code: null,
                date: null,
                reference: null,
                paymentMethodId: null,
                globalDiscount: 0.0,
                globalTotal: 0.0,
                transactionsType: 1,
                transactionsDetails: null,
                numerationId: null,
                commentary: "",
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
            rows: [],
            invoice_subtotal: 0,
            invoice_total: 0,
            invoice_tax: 5,
            list: [
                {
                    id: null,
                    concept: null,
                    transactionsId: null,
                    referenceId: null,
                    description: null,
                    amount: 1,
                    price: 0,
                    discount: 0,
                    total: 0,
                    tax: 0,
                },
            ],
            columns: [
                {
                    label: "Código",
                    field: "code",
                    type: "text",
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

     
    //middleware: "authentication",

    created() {
        this.GetFormRows();
        if (this.$route.query.Action === "edit") {
            this.getTransactionsDetails();
        }
       
    },
    methods: {
        GetFormRows() {

            let FormId = this.$route.query.Form;
            this.DataForm = [];
            this.$axios
                .get(`Form/GetById/${this.$route.query.Form}`)
                .then((response) => {
                    this.DataForm = response.data.data;
                     
                    this.GetFilds();
                  
                })
                .catch((error) => {

                    this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
                });
        },
          GetFilds() {
      this.$axios
        .get(`Formfields/GetByFormId/${this.DataForm.id}`)
        .then((response) => { 
          this.fields =response.data.data;
         
          })           
        .catch((error) => {
          this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
        });
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
        },
        GoBack() {
            this.$router.push({ path: this.Path });
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

        editSchema() {
            this.put(this.principalSchema);
        },
        saveSchema() {
            
                this.ShowModalCreate = false;
                this.principalSchema.transactionsDetails = this.list;
                this.post(this.principalSchema);
            
        },
        async getTransactionsDetails() {
            let url = `Transaction/GetById?id=${this.$route.query.Id}`;
            this.$axios
                .get(url)
                .then((response) => {
                    this.principalSchema = response.data.data;
                    this.list = response.data.data.transactionsDetails;
                    this.calculateTotal();
                })
                .catch((error) => {
                    this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
                });
        },
    
        post(data) {
            data.transactionsType = this.TransactionsType;

            let url = `Transaction/Create`;
            let result = null;

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
                    this.$toast.error(`${result}`, "ERROR", this.izitoastConfig);
                });
        },
        put(data) {
            data.transactionsType = this.DataForm.transactionsType;
            this.$axios
                .put("Transaction/Update", data,)
                .then((response) => {
                    this.$toast.success(
                        "El Registro ha sido actualizado correctamente.",
                        "EXITO"
                    );

                    this.GoBack();
                })
                .catch((error) => {
                    reject(error);
                    this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
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
                                this.$axios
                                    .delete(
                                        `Transaction/Delete/?id=${id}`
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
