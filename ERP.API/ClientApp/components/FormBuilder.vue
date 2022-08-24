<template>

</template>

<script>
export default {

  head() {
    return {
      title: `${this.DataForm.title } | ERP Cardenal Sancha`,
    };
  },
  data() {
    return {

      FormId: "",
      RowId: "",
      DataForm: [],
      DataFormSection: [],
      fields: [],
      principalSchema: {},

    };
  },
}
</script>

<style scoped>

</style>
