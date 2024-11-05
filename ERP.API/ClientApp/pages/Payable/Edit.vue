<template>
  <div>
    <h4> Cuentas por Pagar/Cobrar</h4>
    <ValidationObserver ref="form">
      <form @submit.prevent="onSubmit">
        <div class="row">
          <div class="col-3 p-2">
            <b-button-group class="mt-4 mt-md-0">
              <b-button variant="secondary" class="btn" size="sm" @click="GoBack()">
                <i class="bx bx-arrow-back"></i> Lista
              </b-button>
              <b-button variant="success" title="Guardar" @click="onSubmit" size="sm">
                <i class="uil uil-print font-size-18"></i> Guardar
              </b-button>
            </b-button-group>
          </div>
          <div class="col-lg-12 p-4">
            <div class="card">
              <div class="card-header" style="background-color: white">
                <h5 class="text-center">Editar Cuenta por Pagar</h5>
              </div>
              <div class="card-body justify-content-center container">
                <div class="row">
                  <div class="col-md-6">
                    <b-form-group label="Tipo de Cuenta" label-for="accountType" :label-cols="4">
                      <ValidationProvider rules="required" v-slot="{ errors }">
                        <b-form-select id="accountType" v-model="payable.accountType" :options="accountTypeOptions" size="sm" :state="errors.length > 0 ? false : null" />
                        <span class="text-danger" v-if="errors[0]">Campo requerido</span>
                      </ValidationProvider>
                    </b-form-group>
                  </div>
                  <div class="col-md-6">
                    <b-form-group label="Fecha" label-for="date" :label-cols="4">
                      <ValidationProvider rules="required" v-slot="{ errors }">
                        <b-form-input id="date" v-model="payable.date" type="date" size="sm" :state="errors.length > 0 ? false : null" />
                        <span class="text-danger" v-if="errors[0]">Campo requerido</span>
                      </ValidationProvider>
                    </b-form-group>
                  </div>
                </div>
                <div class="row">
                  <div class="col-md-6">
                    <b-form-group label="Monto" label-for="amount" :label-cols="4">
                      <ValidationProvider rules="required|numeric" v-slot="{ errors }">
                        <b-form-input id="amount" v-model="payable.amount" type="number" size="sm" :state="errors.length > 0 ? false : null" />
                        <span class="text-danger" v-if="errors[0]">Campo requerido</span>
                      </ValidationProvider>
                    </b-form-group>
                  </div>
                  <div class="col-md-6">
                    <b-form-group label="Número de Documento" label-for="documentNumber" :label-cols="4">
                      <ValidationProvider rules="required" v-slot="{ errors }">
                        <b-form-input id="documentNumber" v-model="payable.documentNumber" type="text" size="sm" :state="errors.length > 0 ? false : null" />
                        <span class="text-danger" v-if="errors[0]">Campo requerido</span>
                      </ValidationProvider>
                    </b-form-group>
                  </div>
                </div>
                <div class="row">
                  <div class="col-md-6">
                    <b-form-group label="Contacto" label-for="customerSupplierId" :label-cols="4">
                      <ValidationProvider rules="required" v-slot="{ errors }">
                        <b-form-select id="customerSupplierId" v-model="payable.customerSupplierId" :options="customerSuppliers" size="sm" :state="errors.length > 0 ? false : null" />
                        <span class="text-danger" v-if="errors[0]">Campo requerido</span>
                      </ValidationProvider>
                    </b-form-group>
                  </div>
                  <div class="col-md-6">
                    <b-form-group label="Descripción" label-for="description" :label-cols="4">
                      <b-form-input id="description" v-model="payable.description" type="text" size="sm" />
                    </b-form-group>
                  </div>
                </div>
                <div class="row">
                  <div class="col-md-6">
                    <b-form-group label="Fecha de Vencimiento" label-for="dueDate" :label-cols="4">
                      <b-form-input id="dueDate" v-model="payable.dueDate" type="date" size="sm" />
                    </b-form-group>
                  </div>
                  <div class="col-md-6">
                    <b-form-group label="Método de Pago" label-for="paymentMethodId" :label-cols="4">
                      <b-form-select id="paymentMethodId" v-model="payable.paymentMethodId" :options="paymentMethodOptions" size="sm" />
                    </b-form-group>
                  </div>
                </div>
                <div class="row">
                  <div class="col-md-12">
                    <b-form-group label="Comentario" label-for="commentary" :label-cols="2">
                      <b-form-textarea id="commentary" v-model="payable.commentary" rows="3" size="sm" />
                    </b-form-group>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
        <div class="row">
          <div class="col-3 p-2">
            <b-button-group class="mt-4 mt-md-0">
              <b-button variant="secondary" class="btn" size="sm" @click="GoBack()">
                <i class="bx bx-arrow-back"></i> Lista
              </b-button>
              <b-button variant="success" title="Guardar" @click="onSubmit" size="sm">
                <i class="uil uil-print font-size-18"></i> Guardar
              </b-button>
            </b-button-group>
          </div>
        </div>
      </form>
    </ValidationObserver>
  </div>
</template>

<script>
export default {
  data() {
    return {
      payable: {
        id: null,
        accountType: "payable",
        date: null,
        amount: 0,
        documentNumber: "",
        customerSupplierId: null,
        description: "",
        dueDate: null,
        paymentMethodId: "",
        commentary: ""
      },
      accountTypeOptions: [
        { value: 'payable', text: 'Por Pagar' },
        { value: 'receivable', text: 'Por Cobrar' }
      ],
      customerSuppliers: [],
      paymentMethodOptions: [],
      DataForm: []
    };
  },
  async mounted() {
    await this.fetchCustomerSuppliers();
    await this.fetchPaymentMethods();
    this.loadData();
  },
  methods: {
    async fetchCustomerSuppliers() {
      try {
        const response = await this.$axios.get('Contact/GetAll');
        this.customerSuppliers = response.data.data.map(item => ({ value: item.id, text: item.name }));
      } catch (error) {
        this.$toast.error(error.message, "Error al obtener clientes/proveedores", this.izitoastConfig);
      }
    },
    async fetchPaymentMethods() {
      try {
        const response = await this.$axios.get('PaymentMethod/GetAll');
        this.paymentMethodOptions = response.data.data.map(item => ({ value: item.id, text: item.name }));
      } catch (error) {
        this.$toast.error(error.message, "Error al obtener métodos de pago", this.izitoastConfig);
      }
    },
    GoBack() {
      this.$router.push("/payable");
    },
    async onSubmit() {
      try {
        const isValid = await this.$refs.form.validate();
        if (isValid) {
// Aquí debes obtener el usuario actual para las propiedades deauditoría
          // this.payable.lastModifiedBy = currentUserId;
          await this.$axios.put('Payable/Update', this.payable);
          this.$toast.success("Cuenta por pagar actualizada correctamente.", "Éxito", this.izitoastConfig);
          this.GoBack();
        }
      } catch (error) {
        this.$toast.error(error.message, "Error al actualizar la cuenta por pagar", this.izitoastConfig);
      }
    },
    loadData() {
      const id = this.$route.query.id; // Usamos query en lugar de params para obtener el ID
      if (id) {
        this.fetchPayable(id);
      }
    },
    async fetchPayable(id) {
      try {
        const response = await this.$axios.get(`Payable/GetById?id=${id}`);
        this.payable = response.data.data;
      } catch (error) {
        this.$toast.error(error.message, "Error al obtener la cuenta por pagar", this.izitoastConfig);
      }
    }
  }
};
</script>

<style>
/* Estilos opcionales */
.container {
  width: 90%;
  margin: auto;
}
</style>
