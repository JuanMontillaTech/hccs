<template>
  <div>
    <div class="card">
      <div class="card-body">
    <h1>Cambiar Estado de Recibos</h1>

    <!-- Filtro por estado -->
    <b-form-group label="Filtrar por estado" >
      <b-form-select v-model="selectedStatus" :options="statusOptions" @change="loadReceipts" />
    </b-form-group>

    <!-- Tabla de recibos -->
    <b-table
      :items="receipts"
      :fields="fields"
      selectable
      select-mode="multi"
      @row-selected="onRowSelected"
    >
      <template #cell(selected)="{ rowSelected }">
         Seleccionar recibo
      </template>
    </b-table>

    <!-- Botón para cambiar estado -->
    <b-button @click="openStatusModal" variant="primary" :disabled="selectedReceipts.length === 0">
      Cambiar estado de recibos seleccionados
    </b-button>

    <!-- Modal para seleccionar nuevo estado -->
    <b-modal v-model="isStatusModalOpen" title="Cambiar estado" @ok="updateReceiptStatus">
      <b-form-group label="Confirma que quiere cambiar el estado a completado? ">

      </b-form-group>
    </b-modal>
  </div>  </div>
  </div>
</template>

<script>
export default {
  data() {
    return {
      selectedStatus: '10A8CA88-B198-4241-9F5F-EAD7AE023484', // Estado seleccionado para filtrar
      selectedReceipts: [], // IDs de los recibos seleccionados
      newStatusId: null, // Nuevo estado seleccionado
      isStatusModalOpen: false, // Controla la visibilidad del modal
      receipts: [], // Lista de recibos filtrados
      items:[],
      statusOptions: [
        { value: '10A8CA88-B198-4241-9F5F-EAD7AE023484', text: 'Pendiente' },
        { value: 'D6A2FCF6-D571-4F4B-A53D-D41A7C8A066F', text: 'Completado' },
        { value: '926142F4-0DA8-448F-ACA8-87BA2778CC78', text: 'En espera' },
      ],
      fields: [
        { key: 'selected', label: 'Seleccionar' },
        { key: 'document', label: 'Documento' },
        { key: 'date', label: 'Fecha' },
        { key: 'contact.name', label: 'Contacto' },
        { key: 'total', label: 'Total' },
        { key: 'recipeStatus.name', label: 'Estado' },
      ],
    };
  },
  created() {
    this.loadReceipts();
  },
  methods: {
    // Cargar recibos filtrados por estado
    async loadReceipts() {
      try {
        const response = await this.$axios.get('TransactionReceipt/GetByStatus', {
          params: {
            statusId: this.selectedStatus,
          },
        });

        this.receipts = response.data.data;
      } catch (error) {
        console.error('Error al cargar los recibos:', error);
        this.$toast.error('Error al cargar los recibos.', 'ERROR');
      }
    },

    // Manejar la selección de filas en la tabla
    onRowSelected(items) {
      this.selectedReceipts = items.map((item) => item.id);
    },

    // Abrir el modal para cambiar el estado
    openStatusModal() {
      if (this.selectedReceipts.length === 0) {
        this.$toast.error('Selecciona al menos un recibo.', 'ERROR');
        return;
      }
      this.isStatusModalOpen = true;
    },

    // Actualizar el estado de los recibos seleccionados
    async updateReceiptStatus() {


      try {
        const response = await this.$axios.post('TransactionReceipt/update-status', {
          receiptIds: this.selectedReceipts,
          newStatusId: 'D6A2FCF6-D571-4F4B-A53D-D41A7C8A066F',
        });

        if (response.data.success) {
          this.$toast.success('Estado de los recibos actualizado correctamente.', 'ÉXITO');
          this.loadReceipts(); // Recargar la lista de recibos
        } else {
          this.$toast.error('Error al actualizar el estado de los recibos.', 'ERROR');
        }
      } catch (error) {
        console.error('Error al actualizar el estado de los recibos:', error);
        this.$toast.error('Error al actualizar el estado de los recibos.', 'ERROR');
      } finally {
        this.isStatusModalOpen = false;
      }
    },
  },
  mounted() {
    this.loadReceipts(); // Cargar recibos al iniciar el componente
  },
};
</script>

<style scoped>
/* Estilos personalizados si es necesario */
</style>
