<template>
  <div class="container">
    <b-modal
      size="lg"
      title="Formulario de Concepto"
      v-model="ShowModalCreate"
      hide-footer
    >
      <div class="container">
        <div class="row">
          <div class="col-lg-6 col-md-6 col-sm-12">
            <b-form-group label="Nombre de la cuenta">
              <b-form-input
                v-model="bank.AccountName"
                size="sm"
                :state="$v.bank.AccountName.$error ? false : null"
                trim
              ></b-form-input>
              <p
                class="text-danger text-size-required m-0"
                v-if="$v.bank.AccountName.$error"
              >
                Nombre de la cuenta requerido.
              </p>
            </b-form-group>
          </div>
        </div>
      </div>
    </b-modal>
    <nav class="navbar navbar-default">
      <div class="container-fluid">
        <div class="navbar-header">
          <div>Listado de {{ $options.name }}</div>
        </div>
        <div class="btn-group" role="group" aria-label="Basic example">
          <a
            title="Nuevo Registro"
            @click="showModal()"
            class="btn btn-primary btn-sm text-white"
          >
            <fa icon="file" class="ml-1"></fa>
            Nuevo</a
          >

          <a
            id="_btnRefresh"
            @click="GetAllRows()"
            class="btn btn-light border btn-sm text-black-50 btnRefresh"
            name="_btnRefresh"
            ><i class="fas fa-sync-alt"></i> Actualizar Datos</a
          >
        </div>
      </div>
    </nav>
  </div>
</template>

<script>
import { required } from "vuelidate/lib/validators";
export default {
  name: "Concepto",
  layout: "TheSlidebar",
  data() {
    return {
      ShowModalCreate: false,
      ShowModalEdit: false,
      ShowModalDelete: false,
      ShowModalDetails: false,
      concept: {
        Description: null,
      },
    };
  },
};
</script>

<style></style>
