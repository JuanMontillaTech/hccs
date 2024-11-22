<template>
  <div>
    <div class="card">
      <div class="card-body">
        <h4>Gestión de Cuentas de Usuarios</h4>
    <b-form @submit.prevent="handleSubmit">
      <b-form-group label="Base de Datos" label-for="companySelect">
        <b-form-select id="companySelect" v-model="selectedCompany" :options="companies" @change="fetchUsers" required>
          <template #first>
            <b-form-select-option :value="null" disabled>-- Selecciona una base de datos --</b-form-select-option>
          </template>
        </b-form-select>
      </b-form-group>

      <b-form-group v-if="users.length > 0" label="Usuario" label-for="userSelect">
        <b-form-select id="userSelect" v-model="selectedUser" @change="fetchRoles" required>
          <template #first>
            <b-form-select-option :value="null" disabled>-- Selecciona un usuario --</b-form-select-option>
          </template>
          <b-form-select-option v-for="user in users" :key="user.id" :value="user.id">{{ user.name }}</b-form-select-option>
        </b-form-select>
      </b-form-group>

      <b-form-group v-if="roles.length > 0" label="Rol" label-for="roleSelect">
        <b-form-select id="roleSelect" v-model="selectedRole" @change="fetchForms" required>
          <template #first>
            <b-form-select-option :value="null" disabled>-- Selecciona un rol --</b-form-select-option>
          </template>
          <b-form-select-option v-for="role in roles" :key="role.id" :value="role.id">{{ role.name }}</b-form-select-option>
        </b-form-select>
      </b-form-group>

      <div v-if="forms.length > 0">
        <h5>Formularios asociados al rol:</h5>     <b-button variant="primary" class="btn-sm" @click="addForm">
        <i class="bx bx-plus"></i>
      </b-button>
        <b-list-group>
          <b-list-group-item v-for="form in forms" :key="form.id">
            {{ form.title }}
            <b-button size="sm" variant="danger" @click="removeForm(form.id)" class="float-right btn-sm">
              <i class="bx bx-trash"></i>
            </b-button>
          </b-list-group-item>
        </b-list-group>
        <b-button variant="primary" @click="addForm"  class="btn-sm">
          <i class="bx bx-plus"></i>
        </b-button>
      </div>
    </b-form>

    <b-modal ref="formModal" title="Agregar Formulario" hide-footer>
      <b-form @submit.prevent="saveForm">
        <b-form-group label="Formulario" label-for="modalFormSelect">
          <b-form-select id="modalFormSelect" v-model="selectedFormToAdd" :options="allForms" required></b-form-select>
        </b-form-group>
        <b-button type="submit" variant="primary"  class="btn-sm">Guardar</b-button>
      </b-form>
    </b-modal>
  </div>  </div>  </div>
</template>

<script>
export default {
  data() {
    return {
      companies: [],
      selectedCompany: null,
      users: [],
      selectedUser: null,
      roles: [],
      selectedRole: null,
      forms: [],
      allForms: [],
      selectedFormToAdd: null,
    };
  },
  async mounted() {
    await this.fetchCompanies();
    await this.fetchAllForms(); // Obtener todos los formularios al inicio
  },
  methods: {
    async fetchCompanies() {
      try {
        const response = await this.$axios.get('SysCompany/GetAll');
        this.companies = response.data.data.map(company => ({ value: company.id, text: company.companyName }));
      } catch (error) {
        console.error("Error al obtener compañías:", error);
      }
    },
    async fetchUsers() {
      if (this.selectedCompany) {
        try {
          const response = await this.$axios.get(`SysUserCompany/GetCompanyId?companyId=${this.selectedCompany}`);

          this.users = response.data.data.data.map(userCompany => ({
            id: userCompany.sysUser.email,
            name: userCompany.sysUser.email // Ajusta esto según tu modelo
          }));
          this.roles = [];
          this.forms = [];
        } catch (error) {
          console.error("Error al obtener usuarios:", error);
        }
      } else {
        this.users = [];
      }
    },
    async fetchRoles() {
      if (this.selectedUser) {
        try {
          console.log(this.selectedUser, "Ri")
          const response = await this.$axios.get(`UserRoll/GetByemailUser?email=${this.selectedUser}`);
          this.roles = response.data.data.data.map(userRole => ({
            id: userRole.rollId,
            name: userRole.rolles.name // Ajusta esto según tu modelo
          }));
          this.forms = [];
        } catch (error) {
          console.error("Error al obtener roles:", error);
        }
      } else {
        this.roles = [];
      }
    },
    async fetchForms() {
      if (this.selectedRole) {
        try {
          const response = await this.$axios.get(`RollForm/GetFormId?RollId=${this.selectedRole}`);

          this.forms = response.data.data.data.filter(rollForm => rollForm.froms).map(rollForm => ({
            id: rollForm.id,
            title: rollForm.froms.title // Ajusta esto según tu modelo
          }));
        } catch (error) {
          console.error("Error al obtener formularios:", error);
        }
      } else {
        this.forms = [];
      }
    },
    async fetchAllForms() {
      try {
        const response = await this.$axios.get('Form/GetAll');

        this.allForms = response.data.data.map(form => ({ value: form.id, text: form.title }));

      } catch (error) {
        console.error("Error al obtener todos los formularios:", error);
      }
    },
    addForm() {
      this.selectedFormToAdd = null;
      this.$refs.formModal.show();
    },
    async saveForm() {
      try {
        if (!this.selectedRole || !this.selectedFormToAdd) {
          // Mostrar un mensaje de error si no se ha seleccionado un rol o formulario
          this.$toast.error("Selecciona un rol y un formulario.", "Error", this.izitoastConfig);
          return;
        }

        const data = {
          rollId: this.selectedRole,
          formId: this.selectedFormToAdd,
          lastModifiedDate: new Date(),
          createdDate: new Date(),
          isActive: true,
          // ... otras propiedades necesarias para tu modelo RollFormDto
        };

        await this.$axios.post('RollForm/Create', data);
        this.$toast.success("Formulario agregado correctamente.", "Éxito", this.izitoastConfig);
        await this.fetchForms(); // Actualiza la lista de formularios
        this.$refs.formModal.hide();
      } catch (error) {
        this.$toast.error(error.message, "Error al agregar formulario", this.izitoastConfig);
      }
    },

    async removeForm(formId) {
      try {
        if (!this.selectedRole || !formId) {
          // Mostrar un mensaje de error si no se ha seleccionado un rol o formulario
          this.$toast.error("Selecciona un rol y un formulario.", "Error", this.izitoastConfig);
          return;
        }

        // Buscar el ID de la relación RollForm
        const rollForm = this.forms.find(form => form.id === formId);
        if (!rollForm) {
          this.$toast.error("No se encontró la relación RollForm.", "Error", this.izitoastConfig);
          return;
        }


        const rollFormId = rollForm.id; // Asumiendo que tienes la propiedad rollFormId en el objeto form

        await this.$axios.delete(`RollForm/Delete/${rollFormId}`);
        this.$toast.success("Formulario eliminado correctamente.", "Éxito", this.izitoastConfig);
        await this.fetchForms(); // Actualiza la lista de formularios
      } catch (error) {
        this.$toast.error(error.message, "Error al eliminar formulario", this.izitoastConfig);
      }
    },
    handleSubmit() {
      // Aquí puedes manejar el envío del formulario completo si es necesario
      console.log("Formulario enviado:", this.selectedCompany, this.selectedUser, this.selectedRole, this.forms);
    }
  }
};
</script>
