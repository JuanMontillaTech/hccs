<template>
  <div>

    <FuncionalForm
      :Title="$route.query.Titulo"
      :Form="$route.query.Form"
      :TransactionsType="$route.query.TransactionsType"
      :DateLabel="$route.query.DateLabel"
      :Path="$route.query.Path"
      :IsClient="$route.query.IsClient"
      :Action="$route.query.Action"
      :Id="$route.query.Id"
    ></FuncionalForm>
  </div>
</template>

<script>
export default {
  head() {
    return {
      title: `${this.$route.query.titulo} | ERP Cardenal Sancha`,
    };
  },

  data() {
    return {};
  },

  created() {
    //this.Form  , action: "create",titulo: this.Title, transactionsType : this.TransactionsType, DateLabel:"fecha", path: this.$route.path },
  },
};
</script>
