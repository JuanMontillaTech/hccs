<template>
  <div>
    <div>

      <vueselect
        :options="list"
        :reduce="(row) => row.id"
        :label="Config.sourceLabel"
        v-model="select"
        :name="field"
        @search="onSearch"
        @input="setSelected"
      >
      </vueselect>
    </div>

    <div v-if="Config.formSoportId">
      <button
        type="button"
        v-b-modal.modal-scrollable
        class="btn btn-primary btn-sm waves-effect waves-light"
        data-toggle="modal"
        data-target="#exampleModalScrollable"
      >
        +
      </button>
      <b-modal id="modal-scrollable" scrollable title-class="font-18">
        <FormDynamicExpressAdd
          :FormIdExt="Config.formSoportId"
        ></FormDynamicExpressAdd>
      </b-modal>
    </div>
  </div>
</template>

<script>
export default {
  name: "InfiniteScroll",

  data: () => ({
    list: [],
    observer: null,
    limit: 1000,
    search: "",
    formId: null,
    offset: 0,
  }),
  props: ["Config", "select", "field"],

  computed: {},
  mounted() {
    /**
     * You could do this directly in data(), but since these docs
     * are server side rendered, IntersectionObserver doesn't exist
     * in that environment, so we need to do it in mounted() instead.
     */
    this.formId = this.$route.query.Form;
    this.onSearch("");
    //this.observer = new IntersectionObserver(this.infiniteScroll)
  },
  methods: {
    onSearch(query) {
      this.search = query;

      let url = `${this.Config.sourceApi}?PageNumber=${this.offset}&PageSize=${this.limit}&Search=${this.search}`;

      this.$axios
        .get(`${url}`)
        .then((response) => {

          this.list = response.data.data.data
         })
        .catch((error) => {
          this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
        });
    },

    setSelected(Value) {
      console.log("Valor del componente", Value)
      console.log("campo del componente",  this.field)
      this.$emit("CustomChange", Value);
    },
  },
};
</script>

<style scoped>
.loader {
  text-align: center;
  color: #bbbbbb;
}
</style>
