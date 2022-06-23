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
   this.LoadForm("25f94e8c-8ea0-4ee0-adf5-02149a0e072b");
  },
  methods: {

    LoadForm(id) {
  let url =
        this.$store.state.URL +
        `Form/GetById?id=${id}`;
      this.$axios
        .get(url, {
          headers: {
            "Content-Type": "application/json",
          },
        })
        .then((response) => {
         console.log(response.data)
        })
        .catch((error) => {
          this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
        });
    },
  }
};
</script>
