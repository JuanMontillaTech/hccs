<script>
/**
 * Elements component
 */
export default {
  head() {
    return {
      title: `${this.title} | Nuxtjs Responsive Bootstrap 5 Admin Dashboard`,
    };
  },
  data() {
    return {
      titlePage: "Contabilidad",
      items: [
        {
          text: "Compras",
          href: "/",
        },
        {
          text: "Configuracion",
          active: true,
        },
      ],
      LedgerAccounts: [],
      title: {
        AccountPay: "Pago de cuenta",
        AccountPurchase: "Compra de cuenta",
        AccountPurchaseHolding: "Descuento Ventas",
        AccountTaxholding: "Retención de compra de cuenta",
        RefundAccount: "Cuenta de reembolso",
        AccountAdvance: "Avance de cuenta",
        Commission: "Comisión",
        CommissionExpense: "Comisión por Gastos",
      },
      row: {
        Id: "c7d7a9ef-c28e-4e89-9fb0-894289672403",
        AccountPay: "",
        AccountPurchase: "",
        AccountPurchaseHolding: "",
        AccountTaxholding: "",
        RefundAccount: "",
        AccountAdvance: "",
        Commission: "",
        CommissionExpense: "",
      },
      izitoastConfig: {
        position: "topRight",
      },
    };
  },
  middleware: "authentication",
  created() {
    this.getFirstConfigurationPurchase();
    this.getLeaderAccount();
  },
  methods: {
    async getLeaderAccount() {
      let url = `LedgerAccount/GetAll`;
      let result = null;
      this.$axios
        .get(url)
        .then((response) => {
          result = response;
          this.LedgerAccounts = result.data.data;
        })
        .catch((error) => {
          result = error;
        });
    },
    async getFirstConfigurationPurchase() {
      let url = "ConfigurationPurchase/GetFirst";
      this.$axios
        .get(url)
        .then((response) => {
          console.log(response);
          (this.row.AccountPay = response.data.data.accountPay),
            (this.row.AccountPurchase = response.data.data.accountPurchase),
            (this.row.AccountPurchaseHolding =
              response.data.data.accountPurchaseHolding),
            (this.row.AccountTaxholding = response.data.data.accountTaxholding),
            (this.row.RefundAccount = response.data.data.refundAccount),
            (this.row.AccountAdvance = response.data.data.accountAdvance),
            (this.row.Commission = response.data.data.commission),
            (this.row.CommissionExpense = response.data.data.commissionExpense);
        })
        .catch((error) => {
          this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
        });
    },
    async saveConfigurationPurchase() {
      this.$axios
        .put("ConfigurationPurchase/Update", this.row, {
          headers: {
            "Content-Type": "application/json",
            Authorization: `${localStorage.getItem("token")}`,
          },
        })
        .then((response) => {
          this.$toast.success(
            "La configuracion de venta ha sido guardada correctamente.",
            "EXITO",
            this.izitoastConfig
          );
          this.getFirstConfigurationPurchase();
        })
        .catch((error) => {
          this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
        });
    },
  },
};
</script>

<template>
  <div>
    <PageHeader :title="titlePage" :items="items" />

    <!-- Start Form Sizing -->
    <div class="row">
      <div class="col-lg-12">
        <div class="card mb-5">
          <div class="card-body">
            <form>
              <div class="row">
                <div class="col-lg-6">
                  <div class="form-group">
                    <b-form-group :label="title.AccountPay">
                      <vueselect
                        :options="LedgerAccounts"
                        v-model="row.AccountPay"
                        :reduce="(row) => row.id"
                        label="name"
                      ></vueselect>
                    </b-form-group>
                  </div>
                </div>
                <div class="col-lg-6">
                  <div class="form-group mt-3">
                    <b-form-group :label="title.CommissionExpense">
                      <vueselect
                        :options="LedgerAccounts"
                        v-model="row.CommissionExpense"
                        :reduce="(row) => row.id"
                        label="name"
                      ></vueselect>
                    </b-form-group>
                  </div>
                </div>
              </div>
              <div class="row">
                <div class="col-lg-6">
                  <div class="form-group mt-3">
                    <b-form-group :label="title.AccountPurchase">
                      <vueselect
                        :options="LedgerAccounts"
                        v-model="row.AccountPurchase"
                        :reduce="(row) => row.id"
                        label="name"
                      ></vueselect>
                    </b-form-group>
                  </div>
                </div>

                <div class="col-lg-6">
                  <div class="form-group mt-3 mb-0">
                    <b-form-group :label="title.AccountPurchaseHolding">
                      <vueselect
                        :options="LedgerAccounts"
                        v-model="row.AccountPurchaseHolding"
                        :reduce="(row) => row.id"
                        label="name"
                      ></vueselect>
                    </b-form-group>
                  </div>
                </div>
                <div class="col-lg-6">
                  <div class="form-group mt-3 mb-0">
                    <b-form-group :label="title.AccountTaxholding">
                      <vueselect
                        :options="LedgerAccounts"
                        v-model="row.AccountTaxholding"
                        :reduce="(row) => row.id"
                        label="name"
                      ></vueselect>
                    </b-form-group>
                  </div>
                </div>
                <div class="col-lg-6">
                  <div class="form-group mt-3 mb-0">
                    <b-form-group :label="title.RefundAccount">
                      <vueselect
                        :options="LedgerAccounts"
                        v-model="row.RefundAccount"
                        :reduce="(row) => row.id"
                        label="name"
                      ></vueselect>
                    </b-form-group>
                  </div>
                </div>
                <div class="col-lg-6">
                  <div class="form-group mt-3 mb-0">
                    <b-form-group :label="title.AccountAdvance">
                      <vueselect
                        :options="LedgerAccounts"
                        v-model="row.AccountAdvance"
                        :reduce="(row) => row.id"
                        label="name"
                      ></vueselect>
                    </b-form-group>
                  </div>
                </div>
                <div class="col-lg-6">
                  <div class="form-group mt-3 mb-0">
                    <b-form-group :label="title.Commission">
                      <vueselect
                        :options="LedgerAccounts"
                        v-model="row.Commission"
                        :reduce="(row) => row.id"
                        label="name"
                      ></vueselect>
                    </b-form-group>
                  </div>
                </div>
              </div>
              <b-button
                variant="success"
                class="btn mt-4"
                @click="saveConfigurationPurchase()"
              >
                <i class="bx bx-save"></i> Guardar
              </b-button>
            </form>
          </div>
        </div>
      </div>
    </div>
    <!-- End Form Sizing -->
  </div>
</template>
