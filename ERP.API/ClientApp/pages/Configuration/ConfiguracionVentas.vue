<script>
/**
 * Elements component
 */
export default {
  head() {
    return {
      title: `${this.title} | Nuxtjs Responsive Bootstrap 5 Admin Dashboard`
    };
  },
  data() {
    return {
      titlePage: "Contabilidad",
      items: [{
        text: "Ventas",
        href: "/"
      },
      {
        text: "Configuracion",
        active: true
      }
      ],
      LedgerAccounts: [],
      title:{

          Accountcollect: "Cta. Por Cobrar",
          AccountSelling: "Cta. de Venta",
          DiscountSales: "Descuento Ventas",
          AccountDiscountReceived: "Cta. De Descuento Recibido",
          AccountAdvance: "Cta. De anticipo",
          AccountCheckReturned: "Cta. Cheque Devuelto",
          AccountITBISexpenses: "Cta. Gastos ITBIS",
      },
      row:  
        {
          Accountcollect: "",
          AccountSelling: "",
          DiscountSales: "",
          AccountDiscountReceived: "",
          AccountAdvance: "",
          AccountCheckReturned: "",
          AccountITBISexpenses: "",
        }
      
    };
  },
  // middleware: "authentication",
  created() {

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
  }
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
                      <vueselect :options="LedgerAccounts" v-model="row.Accountcollect"
                        :reduce="(row) => row.id" label="name" ></vueselect>

                    </b-form-group>
                  </div>
                </div>
              </div>
              <div class="row">
                <div class="col-lg-6">
                  <div class="form-group mt-3">

                    <b-form-group :label="title.AccountSelling">
                      <vueselect :options="LedgerAccounts" v-model="row.AccountSelling"
                        :reduce="(row) => row.id" label="name" ></vueselect>

                    </b-form-group>
                  </div>
                </div>

                <div class="col-lg-6">
                  <div class="form-group mt-3 mb-0">
                    <b-form-group :label="title.DiscountSales">
                      <vueselect :options="LedgerAccounts" v-model="row.DiscountSales"
                        :reduce="(row) => row.id" label="name" ></vueselect>

                    </b-form-group>
                  </div>
                </div>
                  <div class="col-lg-6">
                  <div class="form-group mt-3 mb-0">
                    <b-form-group :label="title.AccountDiscountReceived">
                      <vueselect :options="LedgerAccounts" v-model="row.AccountDiscountReceived"
                        :reduce="(row) => row.id" label="name" ></vueselect>

                    </b-form-group>
                  </div>
                </div>
                  <div class="col-lg-6">
                  <div class="form-group mt-3 mb-0">
                    <b-form-group :label="title.AccountAdvance">
                      <vueselect :options="LedgerAccounts" v-model="row.AccountAdvance"
                        :reduce="(row) => row.id" label="name" ></vueselect>

                    </b-form-group>
                  </div>
                </div>
                  <div class="col-lg-6">
                  <div class="form-group mt-3 mb-0">
                    <b-form-group :label="title.AccountCheckReturned">
                      <vueselect :options="LedgerAccounts" v-model="row.AccountCheckReturned"
                        :reduce="(row) => row.id" label="name" ></vueselect>

                    </b-form-group>
                  </div>
                </div>
                  <div class="col-lg-6">
                  <div class="form-group mt-3 mb-0">
                    <b-form-group :label="title.AccountITBISexpenses">
                      <vueselect :options="LedgerAccounts" v-model="row.AccountITBISexpenses"
                        :reduce="(row) => row.id" label="name" ></vueselect>

                    </b-form-group>
                  </div>
                </div>
              </div>
            </form>
          </div>
        </div>
      </div>
    </div>
    <!-- End Form Sizing -->


  </div>
</template>
