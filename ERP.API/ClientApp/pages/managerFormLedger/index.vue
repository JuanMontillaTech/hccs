<template>
  <div>
    <div class="card">
      <div class="card-body">
        <h4>Gestión de Cuentas por Formulario</h4>
        <div class="row">
          <div class="col-md-6 mb-2">
            <b-form-group label="Formulario" label-for="formSelect">
              <b-form-select id="formSelect" v-model="selectedFormId" :options="forms" @change="fetchLedgerAccounts"></b-form-select>
            </b-form-group>
          </div>
        </div>
        <div v-if="selectedFormId">
          <h5>Cuentas asociadas al formulario:</h5>  <b-button variant="primary" @click="addLedgerAccount">
          <i class="bx bx-plus"></i>
        </b-button>
          <b-table striped hover :items="ledgerAccounts" :fields="ledgerAccountFields">
            <template #cell(actions)="row">
              <b-button size="sm" @click="editLedgerAccount(row.item)">
                <i class="bx bx-edit"></i>
              </b-button>
              <b-button size="sm" variant="danger" @click="deleteLedgerAccount(row.item.id)">
                <i class="bx bx-trash"></i>
              </b-button>
            </template>
          </b-table>
          <b-button variant="primary" @click="addLedgerAccount">
            <i class="bx bx-plus"></i>
          </b-button>
        </div>
      </div>
    </div>

    <b-modal ref="ledgerAccountModal" title="Cuenta" hide-footer>
      <form @submit.prevent="saveLedgerAccount">
        <b-form-group label="Cuenta" label-for="ledgerAccountSelect">
          <b-form-select id="ledgerAccountSelect" v-model="selectedLedgerAccountId" :options="allLedgerAccounts"></b-form-select>
        </b-form-group>
        <b-button type="submit" variant="primary">Guardar</b-button>
      </form>
    </b-modal>
  </div>
</template>

<script>
import Swal from "sweetalert2";

export default {
  data() {
    return {
      selectedFormId: null,
      forms: [],
      rowId: null,
      ledgerAccounts: [],
      ledgerAccountFields: [
        { key: 'name', label: 'Nombre' },
        { key: 'code', label: 'Código' },
        { key: 'actions', label: 'Acciones' }
      ],
      allLedgerAccounts: [],
      selectedLedgerAccountId: null,
    };
  },
  async mounted() {
    await this.fetchForms();
    await this.fetchAllLedgerAccounts();
  },
  methods: {
    async fetchForms() {
      try {
        const response = await this.$axios.get('Form/GetAllHss');
        this.forms = response.data.data.map(form => ({ value: form.id, text: form.title }));
      } catch (error) {
        this.$toast.error(error.message, "Error al obtener formularios", this.izitoastConfig);
      }
    },
    async fetchLedgerAccounts() {
      if (this.selectedFormId) {
        try {
          const response = await this.$axios.get(`FormLedgerAccount/GetByFormIdHccs?formId=${this.selectedFormId}`);
          this.ledgerAccounts = response.data.data.map(item => ({
            ...item,
            name: item.ledgerAccount.name,
            code: item.ledgerAccount.code
          }));

        } catch (error) {
          this.$toast.error(error.message, "Error al obtener cuentas", this.izitoastConfig);
        }
      } else {
        this.ledgerAccounts = [];
      }
    },
    async fetchAllLedgerAccounts() {
      try {
        const response = await this.$axios.get('LedgerAccount/GetAll');
        this.allLedgerAccounts = response.data.data.map(account => ({ value: account.id, text: `${account.code} - ${account.name}` }));
      } catch (error) {
        this.$toast.error(error.message, "Error al obtener cuentas", this.izitoastConfig);
      }
    },
    addLedgerAccount() {
      this.rowId = null
      this.selectedLedgerAccountId = null;
      this.$refs.ledgerAccountModal.show();
    },
    editLedgerAccount(item) {
      this.selectedLedgerAccountId = item.ledgerAccountId;
      this.rowId = item.id;
      this.$refs.ledgerAccountModal.show();
    },
    async saveLedgerAccount() {
      try {
        if (this.selectedLedgerAccountId) {

          const data = {
            ledgerAccountId: this.selectedLedgerAccountId,
            formId: this.selectedFormId,
            id:   this.rowId,
            lastModifiedDate: new Date(),
            createdDate: new Date(),
            isActive: true
          };

          if (this.rowId === null || this.rowId === undefined) {
            await this.$axios.post('FormLedgerAccount/Create', data);
          }else
          {
            console.log(data)
            await this.$axios.put('FormLedgerAccount/Update', data);
            this.rowId = null
          }


          this.$toast.success("Cuenta agregada correctamente.", "Éxito", this.izitoastConfig);
          await this.fetchLedgerAccounts();
        }
      } catch (error) {
        this.$toast.error(error.message, "Error al agregar cuenta", this.izitoastConfig);
      } finally {
        this.$refs.ledgerAccountModal.hide();
      }
    },
    async deleteLedgerAccount(id) {
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
          // Aquí debes obtener el usuario actual para las propiedades de auditoría
          // const currentUserId = ...;

          const data = {
            id: id,
            // lastModifiedBy: currentUserId,
            lastModifiedDate: new Date(),
            isActive: false
          };

          await this.$axios.put('FormLedgerAccount/Update', data); // Asumiendo que tienes un endpoint para actualizar
          this.$toast.success("Cuenta eliminada correctamente.", "Éxito", this.izitoastConfig);
          await this.fetchLedgerAccounts();
        }
      } catch (error) {
        this.$toast.error(error.message, "Error al eliminar la cuenta", this.izitoastConfig);
      }
    }
  }
};
</script>

<style>
/* Estilos opcionales */
</style>
