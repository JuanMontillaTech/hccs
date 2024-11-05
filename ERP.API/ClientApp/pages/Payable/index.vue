<template>
  <div>
    <div class="card">
      <div class="card-body">
        <h4>Listado de Cuentas por Pagar/Cobrar</h4>
        <div class="row">
          <div class="col-md-4 mb-2">
            <input type="text" class="form-control" placeholder="Buscar..." v-model="searchQuery" @input="searchPayables">
          </div>
          <div class="col-md-8 mb-2 text-right">
            <button class="btn btn-primary" @click="createNewPayable">
              <i class="bx bx-plus"></i> Nueva
            </button>
          </div>
        </div>
        <b-table striped hover :items="payables" :fields="fields" :per-page="perPage" :current-page="currentPage"
                 :filter="searchQuery">
          <template #cell(actions)="row">
            <b-button size="sm" @click="editPayable(row.item.id)">
              <i class="bx bx-edit"></i>
            </b-button>
            <b-button size="sm" variant="danger" @click="deletePayable(row.item.id)">
              <i class="bx bx-trash"></i>
            </b-button>
          </template>
        </b-table>
        <b-pagination v-model="currentPage" :total-rows="totalRows" :per-page="perPage" align="center"></b-pagination>
      </div>
    </div>
  </div>
</template>

<script>
import Swal from "sweetalert2";
import moment from "moment/moment";
import numbro from "numbro";

export default {
  data() {
    return {
      payables: [],
      fields: [
        { key: 'documentNumber', label: 'Número de Documento' },
        { key: 'customerSupplierName', label: 'Contacto' },
        { key: 'date', label: 'Fecha' },
        { key: 'amount', label: 'Monto' },
        { key: 'description', label: 'Descripción' },
        { key: 'dueDate', label: 'Fecha de Vencimiento' },
        { key: 'paymentMethodId', label: 'Método de Pago' },
        { key: 'actions', label: 'Acciones' }
      ],
      searchQuery: "",
      currentPage: 1,
      perPage: 10,
      totalRows: 0
    };
  },
  async mounted() {
    await this.fetchPayables();
  },
  methods: {
    FormatMoney(amount) {
      return numbro(amount).format("$0,0.00");
    },


    async fetchPayables() {
      try {
        const response = await this.$axios.get('Payable/GetAll');
        this.payables = await Promise.all(response.data.data.map(async payable => {
             console.log(payable)
          return {
            ...payable,
            customerSupplierName: payable.contact.name ,
            date: this.FormatDate(payable.date), // Formatear la fecha
            dueDate: this.FormatDate(payable.dueDate), // Formatear la fecha
            amount: this.FormatMoney(payable.amount),
            paymentMethodId:  payable.paymentMethod.name,


            // ... otros campos ...
          };
        }));
        this.totalRows = this.payables.length;
      } catch (error) {
        // Manejo de errores
      }
    },

    createNewPayable() {
      this.$router.push("/payable/create");
    },
    FormatDate(date) {
      return moment(date).lang("es").format("DD/MM/YYYY");
    },
    editPayable(id) {
      this.$router.push(`/payable/Edit?id=${id}`);
    },
    async deletePayable(id) {
      try {
        const result = await Swal.fire({
          title: '¿Estás seguro?',
          text: "Esta acción no se puede deshacer.",
          icon: 'warning',
          showCancelButton: true,
          confirmButtonColor: '#3085d6',
          cancelButtonColor: '#d33',
          confirmButtonText: 'Sí, eliminar',
          cancelButtonText: 'Cancelar'
        });

        if (result.isConfirmed) {
          await this.$axios.delete(`Payable/Delete/${id}`);
          this.$toast.success("Cuenta por pagar eliminada correctamente.", "Éxito", this.izitoastConfig);
          await this.fetchPayables();
        }
      } catch (error) {
        this.$toast.error(error.message, "Error al eliminar la cuenta por pagar", this.izitoastConfig);
      }
    },
    searchPayables() {
      // La búsqueda se realiza automáticamente por la directiva `v-model` en el input
    }
  }
};
</script>

<style>
/* Estilos opcionales */
</style>
