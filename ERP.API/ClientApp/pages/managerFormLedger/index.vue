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
          <b-table striped hover :items="ledgerAccounts.slice().sort((a, b) => a.index - b.index)" :fields="ledgerAccountFields">
            <template #cell(actions)="row">
              <b-button size="sm" variant="primary"   @click="editLedgerAccount(row.item)">
                <i class="bx bx-edit"></i>
              </b-button>
              <b-button size="sm" variant="danger" @click="deleteLedgerAccount(row.item.id)">
                <i class="bx bx-trash"></i>
              </b-button>
              <b-button size="sm"   variant="info" @click="moveLedgerAccountUp(row.item)">
                <i class="bx bx-down-arrow-alt"></i>
              </b-button>
              <b-button size="sm" variant="info"   @click="moveLedgerAccountDown(row.item)">
                <i class="bx bx-up-arrow-alt"></i>
              </b-button>
            </template>
          </b-table>
          <b-button variant="primary"  @click="addLedgerAccount">
            <i class="bx bx-plus"></i>
          </b-button>
        </div>
      </div>
    </div>

    <b-modal ref="ledgerAccountModal" title="Cuenta" hide-footer>
      <form @submit.prevent="saveLedgerAccount">
        <b-form-group label="Cuenta" label-for="ledgerAccountSelect">

          <vueselect
            :options="allLedgerAccounts"
            :reduce="(row) => row.value"
            label="text"
            v-model="selectedLedgerAccountId"
          >
          </vueselect>

        </b-form-group>
        <b-form-group label="Orden" label-for="ledgerAccountSelect">
          <b-input id="ledgerAccountSelect" v-model="selectedLedgerOrden" ></b-input>
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
        { key: 'index', label: 'Orden' },
        { key: 'actions', label: 'Acciones' }
      ],
      allLedgerAccounts: [],
      selectedLedgerAccountId: null,
      selectedLedgerOrden: 0,
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
            isActive: true,
            Index: this.selectedLedgerOrden,
          };

          if (this.rowId === null || this.rowId === undefined) {
            await this.$axios.post('FormLedgerAccount/Create', data);
          }else
          {

            await this.$axios.put('FormLedgerAccount/Update', data);
            this.rowId = null
          }

          this.$toast.success("Guardado.", "Éxito", this.izitoastConfig);
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
    },
    async moveLedgerAccountUp(item) {
      const currentIndex = this.ledgerAccounts.findIndex(account => account.id === item.id);
      if (currentIndex > 0) {
        // Encontrar el índice del elemento anterior

          this.ledgerAccounts[currentIndex].index = this.ledgerAccounts[currentIndex].index + 1;

        // Actualizar en el backend
        await this.updateLedgerAccount(this.ledgerAccounts[currentIndex]);


        // Refrescar la tabla
        this.fetchLedgerAccounts();
      }
    },

    async moveLedgerAccountDown(item) {
      const currentIndex = this.ledgerAccounts.findIndex(account => account.id === item.id);
      if (currentIndex < this.ledgerAccounts.length - 1) {
        // Encontrar el índice del elemento siguiente
        this.ledgerAccounts[currentIndex].index = this.ledgerAccounts[currentIndex].index - 1;

        // Actualizar en el backend
        await this.updateLedgerAccount(this.ledgerAccounts[currentIndex]);


        // Refrescar la tabla
        this.fetchLedgerAccounts();
      }
    },

    async updateLedgerAccount(account) {
      try {
        const data = {
          ledgerAccountId: account.ledgerAccountId,
          formId: this.selectedFormId,
          id: account.id,
          lastModifiedDate: new Date(),
          createdDate: new Date(),
          isActive: true,
          index: account.index,
        };
        await this.$axios.put('FormLedgerAccount/Update', data);
      } catch (error) {
        this.$toast.error(error.message, "Error al actualizar la cuenta", this.izitoastConfig);
      }
    }
  }
};
</script>
