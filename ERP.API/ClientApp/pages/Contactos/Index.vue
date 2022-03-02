<template>
  <div>
    <div class="card">
      <div class="card-header bg-Cprimary">Listado</div>
      <div class="card-body">
        <div class="btn-group" role="group" aria-label="Basic example">
          <a
            title="Nuevo Registro"
            v-on:click="ShowModelCreate = true"
            class="btn btn-primary btn-sm text-white"
            ><i class="fas fa-file"></i> Nuevo</a
          >

          <a
            id="_btnRefresh"
            v-on:click="getAllRows"
            class="btn btn-light btn-sm text-black-50 btnRefresh"
            name="_btnRefresh"
            ><i class="fas fa-sync-alt"></i> Actualizar Datos</a
          >
        </div>

        <div class="btn-group" role="group" aria-label="Basic example"></div>
        <div class="row justify-content-center" style="margin-top: 0%">
          <div class="col-md-12">
            <div class="card">
              <div class="card-header text-left bg-light text-black-50">
                <i class="fas fa-list-ul"></i>
              </div>
              <div class="card-body bg-light">
                <div class="row">
                  <div
                    class="col-md-12 table-responsive table-responsive-xl table-responsive-sm"
                  >
                    <table class="table tableDynamic striped table-border">
                      <thead>
                        <tr>
                          <th></th>
                          <th></th>
                        </tr>
                      </thead>
                      <tr
                        v-for="(Contact, index) in Contacts"
                        v-bind:key="index"
                      >
                        <td>
                          <a
                            href="#"
                            v-on:click="removeContact(Contact, index)"
                            class="btn btn-light btn-sm"
                            ><i class="fas fa-trash-alt"></i
                          ></a>
                          <a
                            href="#"
                            class="btn btn-light btn-sm"
                            v-on:click="editRecordShow(Contact, index)"
                            ><i class="fas fa-edit"></i
                          ></a>
                        </td>
                        <td>Contact.name</td>
                      </tr>
                    </table>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
    <b-modal
      id="newModal"
      v-model="ShowModelCreate"
      title="Agregar Registro"
      hide-footer
    >
      <div class="form-group">
        <label for="name">Nombre</label>
        <input
          v-model="name"
          type="text"
          class="form-control"
          style="width: 100%"
        />
      </div>

      <div class="modal-footer">
        <button
          type="button"
          class="btn btn-light btn-sm text-black-50"
          v-on:click="ShowModelCreate = false"
        >
          <i class="fas fa-list"></i> Cerrar
        </button>
        <button
          type="button"
          class="btn btn-success btn-sm text-white btnSave"
          v-on:click="addNewRecord"
        >
          <i class="fas fa-save"></i> Guardar
        </button>
      </div>
    </b-modal>
    
      <b-modal
      id="EditModal"
      v-model="ShowModelEdit"
      title="Agregar Registro"
      hide-footer
    >
       
            <div class="form-group">
              <label for="name">nombre</label>
              <input
                v-model="name"
                type="text"
                class="form-control"
                style="width: 100%"
              />
            </div>
         
        
            <button
              type="button"
              class="btn btn-light btn-sm text-black-50"
              data-dismiss="modal"
            >
              <i class="fas fa-list"></i> Cerrar
            </button>
            <button
              type="button"
              class="btn btn-success btn-sm text-white btnSave"
              v-on:click="EditRecord"
            >
              <i class="fas fa-save"></i> Guardar Cambio
            </button>
 
         
       
    </b-modal>  
  </div>
</template>

<script>
export default {
  data() {
    return {
      ShowModelCreate: false,
      ShowModelEdit: false,
      ShowModelDelete: false,
      Contacts: [],
      name: "",
      editItemIndex: null,
      editItemId: null,
    };
  },
  created: function () {
    this.getAllRows();
  },
  methods: {
    clearData: function () {
      var vm = this;
      vm.name = "";

      vm.editItemIndex = null;
      vm.editItemId = null;
    },
    editRecordShow: function (item, index) {
      this.name = item.name;
      editItemIndex = index;
      editItemId = item.id;
      //$("#editModal").modal("show");
    },
    EditRecord: function (item) {
       
      var newContact = {
        id: editItemId,
        name: vm.name,
      };
      //$.ajax({ url: "/Contact", data: newContact, method: "PUT" })
      // .done(function () {
      //   vm.getAllRows();
      //   toastr.success("Registro actualizado.");
      // })
      // .fail(function () {
      //   toastr.error("No pudo Acualizar el registro");
      // })
      // .always(function () {
      //   vm.clearData();
      // });
     this.ShowModelEdit = false;
    },
    removeContact: function (item, index) {
      var vm = this;
      var recordDelete = {
        id: item.id,
      };
      //$.ajax({ url: "/Contact", data: recordDelete, method: "DELETE" })
      // .done(function (data) {
      //   vm.getAllRows();
         //toastr.success("Registro removido");
      // })
      // .fail(function () {
      //   toastr.error("No pudo remover el registro!");
      // });
    },
    dateFormart: function (date) {
      return moment(date).lang("es").format("MMMM(YYYY)");
    },
    MoneyFormart: function (money) {
      return numeral(money).format("//$0,0.00");
    },
    addNewRecord: function () {
      var vm = this;
      var newRecord = {
        name: vm.name,
      };

      //$.ajax({ url: "/Contact", data: newRecord, method: "POST" })
      // .done(function (data) {
      //   vm.Contacts.splice(0, 0, newRecord);
      //   toastr.success("Nuevo registro agregado.");
      // })
      // .fail(function () {
      //   toastr.error("No pudo agregar el registro!");
      // })
      // .always(function () {
      //   vm.clearData();
      // });
    this.showModal = false;
    },
    getAllRows: function () {
     
      //$.ajax({ url: "/Contact", method: "GET" })
      // .done(function (data) {
      //   vm.Contacts = data;
      //   toastr.success("Todo el registro cargado.");
      // })
      // .fail(function () {
      //   toastr.error("No pudo cargar todos los registro!");
      // });
    },
    showModal: function () {
      
      this.clearData();
      this.ShowModelCreate = true;
      //$("#newModal").modal("show");
    },
  },
};
</script>

<style></style>
