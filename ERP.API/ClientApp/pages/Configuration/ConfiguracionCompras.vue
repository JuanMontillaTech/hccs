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
        Accountcollect: "Cta. Por Cobrar",
        AccountSelling: "Cta. de Venta",
        DiscountSales: "Descuento Ventas",
        AccountDiscountReceived: "Cta. De Descuento Recibido",
        AccountAdvance: "Cta. De anticipo",
        AccountCheckReturned: "Cta. Cheque Devuelto",
        AccountITBISexpenses: "Cta. Gastos ITBIS",
      },
      row: {
        Id: "c7d7a9ef-c28e-4e89-9fb0-894289672403",
        Accountcollect: "",
        AccountSelling: "",
        DiscountSales: "",
        AccountDiscountReceived: "",
        AccountAdvance: "",
        AccountCheckReturned: "",
        AccountITBISexpenses: "",
      },
      izitoastConfig: {
        position: "topRight",
      },
    };
  },
  middleware: "authentication",
  created() {
    this.getLeaderAccount();
    this.getFirstConfigurationSell();
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
    async getFirstConfigurationSell() {
      let url = "ConfigurationSell/GetFirst";
      this.$axios
        .get(url)
        .then((response) => {
          (this.row.Accountcollect = response.data.data.accountcollect),
            (this.row.AccountSelling = response.data.data.accountSelling),
            (this.row.DiscountSales = response.data.data.discountSales),
            (this.row.AccountDiscountReceived =
              response.data.data.accountDiscountReceived),
            (this.row.AccountAdvance = response.data.data.accountAdvance),
            (this.row.AccountCheckReturned =
              response.data.data.accountCheckReturned),
            (this.row.AccountITBISexpenses =
              response.data.data.accountITBISexpenses),
            console.log(response.data.data);
        })
        .catch((error) => {
          this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
        });
    },
    async saveConfigurationSell() {
      this.$axios
        .put("ConfigurationSell/Update", this.row, {
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
          this.getFirstConfigurationSell();
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
                    <b-form-group :label="title.Accountcollect">
                      <vueselect
                        :options="LedgerAccounts"
                        v-model="row.Accountcollect"
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
                    <b-form-group :label="title.AccountSelling">
                      <vueselect
                        :options="LedgerAccounts"
                        v-model="row.AccountSelling"
                        :reduce="(row) => row.id"
                        label="name"
                      ></vueselect>
                    </b-form-group>
                  </div>
                </div>

                <div class="col-lg-6">
                  <div class="form-group mt-3 mb-0">
                    <b-form-group :label="title.DiscountSales">
                      <vueselect
                        :options="LedgerAccounts"
                        v-model="row.DiscountSales"
                        :reduce="(row) => row.id"
                        label="name"
                      ></vueselect>
                    </b-form-group>
                  </div>
                </div>
                <div class="col-lg-6">
                  <div class="form-group mt-3 mb-0">
                    <b-form-group :label="title.AccountDiscountReceived">
                      <vueselect
                        :options="LedgerAccounts"
                        v-model="row.AccountDiscountReceived"
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
                    <b-form-group :label="title.AccountCheckReturned">
                      <vueselect
                        :options="LedgerAccounts"
                        v-model="row.AccountCheckReturned"
                        :reduce="(row) => row.id"
                        label="name"
                      ></vueselect>
                    </b-form-group>
                  </div>
                </div>
                <div class="col-lg-6">
                  <div class="form-group mt-3 mb-0">
                    <b-form-group :label="title.AccountITBISexpenses">
                      <vueselect
                        :options="LedgerAccounts"
                        v-model="row.AccountITBISexpenses"
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
                @click="saveConfigurationSell()"
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
