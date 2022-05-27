<script>
import { tsNullKeyword } from "@babel/types";
var numbro = require("numbro");
var moment = require("moment");

/**
 * Invoice Detail component
 */
export default {
  head() {
    return {
      title: `${this.title} | Nuxtjs Responsive Bootstrap 5 Admin Dashboard`,
    };
  },
  data() {
    return {
      labelPerson: "",
      title: "Invoice",
      id: null,
      items: [
        {
          text: "Invoices",
        },
        {
          text: "Invoice Detail",
          active: true,
        },
      ],
      client: {
        name: "",
        adress: "",
        email: "",
        phone1: "",
        isClient: null,
        isSupplier: null,
        isEmployee: null,
        isSister: null,
      },
      invoice: { contact: {} },
    };
  },
  created() {
    this.LoadData();
  },
  methods: {
    LoadData() {
      switch (this.$route.query.type) {
        case "facturaingreso":
          this.title = "Factura de Ingreso";
          this.labelPerson = "Cliente";
          break;
        case "cotizacion":
          this.title = "Cotización";
          this.labelPerson = "Cliente";
          break;
        case "notasDeCredito":
          this.title = "Nota de Crédito";
          this.labelPerson = "Cliente";
          break;
        case "conduce":
          this.title = "Conduce";
          this.labelPerson = "Cliente";
          break;
        case "notasDeDebito":
          this.title = "Nota de Débito";
          this.labelPerson = "Proveedor";
          break;
        case "ordenDeCompra":
          this.title = "Orden de Compra";
          this.labelPerson = "Proveedor";
          break;
        case "facturascompras":
          this.title = "Facturas de Compras";
          this.labelPerson = "Proveedor";
          break;
        default:
          break;
      }
      this.id = this.$route.query.id;
      this.getInvoiceDetails();
    },
    SetTotal(globalTotal) {
      return numbro(globalTotal).format("0,0.00");
    },
    GetDate(date) {
      return moment(date).lang("es").format("DD/MM/YYYY");
    },

    getInvoiceDetails() {
      let url =
        this.$store.state.URL + `Transaction/GetAllDataById?id=${this.id}`;

      this.$axios
        .get(url, {
          headers: {
            "Content-Type": "application/json",
          },
        })
        .then((response) => {
          let responseData = response.data.data;
          this.invoice = responseData;
          this.invoice.date = this.GetDate(this.invoice.date);
          this.invoice.globalTotal = this.SetTotal(this.invoice.globalTotal);
          console.log("invoice", responseData);
        })
        .catch((error) => {
          console.log(error);
        });
    },
  },
};
</script>

<template>
  <div>
    <PageHeader :title="title" :items="items" />

    <div class="row">
      <div class="col-lg-12">
        <div class="card">
          <div class="card-body">
            <div class="invoice-title">
              <h4 class="float-end font-size-16">
                {{ title }} #{{invoice.code}}
                <!-- <span class="badge badge-success font-size-12 ml-2">Paid</span> -->
              </h4>
              <div class="mb-4">
                <img
                  src="~/assets/images/logo-dark.png"
                  alt="logo"
                  height="20"
                />
              </div>
              <div class="text-muted">
                <p class="mb-1">641 Counts Lane Wilmore, KY 40390</p>
                <p class="mb-1">
                  <i class="uil uil-envelope-alt mr-1"></i> abc@123.com
                </p>
                <p><i class="uil uil-phone mr-1"></i> 012-345-6789</p>
              </div>
            </div>

            <hr class="my-4" />

            <div class="row">
              <div class="col-sm-6">
                <div class="text-muted">
                  <h5 class="font-size-16 mb-3">{{ labelPerson }}</h5>
                  <h5 class="font-size-15 mb-2">{{ invoice.contact.name }}</h5>
                  <p class="mb-1">{{ invoice.contact.address }}</p>
                  <p class="mb-1">{{ invoice.contact.email }}</p>
                  <p>{{ invoice.contact.phone1 }}</p>
                </div>
              </div>
              <div class="col-sm-6">
                <div class="text-muted text-sm-right">
                  <div>
                    <h5 class="font-size-16 mb-1">{{ title }} No:</h5>
                    <p>#{{ invoice.code }}</p>
                  </div>
                  <div class="mt-4">
                    <h5 class="font-size-16 mb-1">{{ title }} Fecha:</h5>
                    <p>{{ invoice.date }}</p>
                  </div>
                  <!-- <div class="mt-4">
                                    <h5 class="font-size-16 mb-1">Order No:</h5>
                                    <p>#1123456</p>
                                </div> -->
                </div>
              </div>
            </div>

            <div class="py-2">
              <h5 class="font-size-15">Detalles</h5>

              <div class="table-responsive">
                <table class="table table-nowrap table-centered mb-0">
                  <thead>
                    <tr>
                      <th style="width: 70px">No.</th>
                      <th>Concepto</th>
                      <th>Precio</th>
                      <th>Cantidad</th>
                      <th class="text-right" style="width: 120px">Total</th>
                    </tr>
                  </thead>
                  <tbody>
                    <tr
                      v-for="(row, idx) in invoice.transactionsDetails"
                      :key="row.id"
                      
                    >
                      <th scope="row">{{ idx + 1 }}</th>
                      <td>
                        <h5 class="font-size-15 mb-1">
                          {{ row.concept.reference }}
                        </h5>
                        <ul class="list-inline mb-0">
                          <li class="list-inline-item">
                            <span class="fw-medium"
                              >{{ row.concept.description }}
                            </span>
                          </li>
                        </ul>
                      </td>
                      <td>{{ row.amount }}</td>
                      <td>{{ row.total }}</td>
                      <td class="text-right">$260.00</td>
                    </tr>

                    <tr>
                      <th scope="row" colspan="4" class="text-right">
                        Sub Total
                      </th>
                      <td class="text-right">RD${{ invoice.globalTotal }}</td>
                    </tr>
                    <tr>
                      <th scope="row" colspan="4" class="border-0 text-right">
                        Descuento :
                      </th>
                      <td class="border-0 text-right">
                        - RD${{ invoice.globalDiscount }}
                      </td>
                    </tr>
                    <!-- <tr>
                                        <th scope="row" colspan="4" class="border-0 text-right">
                                            Shipping Charge :
                                        </th>
                                        <td class="border-0 text-right">$25.00</td>
                                    </tr> -->
                    <tr>
                      <th scope="row" colspan="4" class="border-0 text-right">
                        Impuesto:
                      </th>
                      <td class="border-0 text-right">RD$13.00</td>
                    </tr>
                    <tr>
                      <th scope="row" colspan="4" class="border-0 text-right">
                        Total
                      </th>
                      <td class="border-0 text-right">
                        <h4 class="m-0">RD${{ invoice.globalTotal }}</h4>
                      </td>
                    </tr>
                  </tbody>
                </table>
              </div>
              <div class="d-print-none mt-4">
                <div class="float-end">
                  <a
                    href="javascript:window.print()"
                    class="btn btn-success waves-effect waves-light mr-1"
                  >
                    <i class="fa fa-print"></i>
                  </a>
                  <!-- <a
                    href="#"
                    class="btn btn-primary w-md waves-effect waves-light"
                    >Send</a
                  > -->
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
    <!-- end row -->
  </div>
</template>
