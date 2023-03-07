<template>
  <div>
    <div>

      <vueselect
        :options="list"
        :reduce="(row) => row.id"
        :label="field.sourceLabel"
        v-model="select"
        :name="field.field"
        @search="onSearch"
        @input="setSelected"
        placeholder="Escriba 3 o mas digitos para buscar"
      >
      <template v-slot:option="option" >
                          <span>
                            {{ option.name }} <strong v-if=" option.phone1"> Tel:  {{ option.phone1 }}   {{ option.phone2 }} </strong>

                          </span>

                        </template>
      </vueselect>
    </div>

    <div v-if="field.formSoportId">
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
          :FormIdExt="field.formSoportId"
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

    limit: 0,
    formId: null,
    search: "",
    offset: 0,
  }),
  props: ["field", "select", "Index"],

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
      let QuertyLimit = 3;
      if (query.length >= 3)
      {
        this.limit = 5;

      this.search = query;

      let url = `${this.field.sourceApi}?PageNumber=${this.offset}&PageSize=${this.limit}&Search=${this.search}`;

      this.$axios
        .get(`${url}`)
        .then((response) => {
        (this.list = response.data.data.data);
        })
        .catch((error) => {
          this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
        });
    } else {
      this.limit = 0;
    }

    },

    setSelected(value) {
      this.$emit("CustomChange", this.field.field, value, this.Index);
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
