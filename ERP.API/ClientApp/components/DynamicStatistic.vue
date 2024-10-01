<template>
  <div>

    <div class="row">
      <div class="col-lg-6">
        <DynamicStatisticComponents
          v-for="(fieldsRow, fieldIndex) in firstHalfListStatistic"
          :key="fieldIndex"
          :FormId="fieldsRow.formId"
          :ReportQueryId="fieldsRow.reportQueryId"
          :Title="fieldsRow.reportQuery.Name"
          :ConfigId="fieldsRow.configId"
        />
      </div>
      <div class="col-lg-6">
        <DynamicStatisticComponents
          v-for="(fieldsRow, fieldIndex) in secondHalfListStatistic"
          :key="fieldIndex"
          :FormId="fieldsRow.formId"
          :ReportQueryId="fieldsRow.reportQueryId"
          :Title="fieldsRow.reportQuery.Name"
          :ConfigId="fieldsRow.configId"
        />
      </div>
    </div>
  </div>
</template>

<script>
import { computed } from "vue";
import DynamicStatisticComponents from "./DynamicStatisticComponents.vue";

export default {
  layout: "horizontal-citizen",
  head() {
    return {
      title: computed(() => `Reporte ${this.DataForm.Title}`),
      PanelDix: true,
    };
  },
  components: {
    DynamicStatisticComponents,
  },
  data() {
    return {
      FormId: "",
      ListStatistic: [],
    };
  },
  props: {
    FormId: {
      type: String,
      required: true,
    },
  } ,

  created() { 
    if (this.FormId) {
      
      this.getStatistic();
    }
  },
  methods: {
    async getStatistic() {
      if (!this.FormId) return;
      const url = `Statistic/GetAll`;
      try {
        const response = await this.$axios.get(url); 
        this.ListStatistic = response.data.data || [];
      } catch (error) {
        this.ListStatistic = [];
      }
    },
  },
  computed: {
    shouldPrint() {
      return window.matchMedia("print").matches;
    },
    firstHalfListStatistic() {
      const halfIndex = Math.ceil(this.ListStatistic.length / 2);
      return this.ListStatistic.slice(0, halfIndex);
    },
    secondHalfListStatistic() {
      const halfIndex = Math.ceil(this.ListStatistic.length / 2);
      return this.ListStatistic.slice(halfIndex);
    },
  },
};
</script>

<style></style>
