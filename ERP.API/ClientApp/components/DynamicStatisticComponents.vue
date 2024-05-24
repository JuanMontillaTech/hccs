<template>
  <div class="card-body">
    <div class="d-print-none mt-4">
      <div class="card">
        <div class="card-body">
          <h4>{{ Title }}</h4>
          <component
            :is="selectedChart"
            v-if="chartDataAvailable"
            :titleHeader="selectedChart === 'BarChart' ? Title : ''"
            :height="selectedChart === 'BarChart' ? 300 : 140"
            :labels="labels1"
            :dataDashboard="data1"
            bgColor="rgba(0, 151, 255, 0.8)"
            bgHoveColor="rgba(0, 151, 255, 0.8)"
          />
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import BarChart from "./BarChart";
import DonutChart from "./DonutChart";
import PieChart from "./PieChart";
import PolarChart from "./PolarChart";
export default {
  layout: "horizontal-citizen",
  head() {
    return {
      title: `Reporte ${this.DataForm.Title}`,
    };
  },
  components: {
    BarChart,
    DonutChart,
    PieChart,
    PolarChart,
  },
  data() {
    return {
      ConfigSet: {
        DonutChart: "4237e84f-298c-47eb-9256-099e9795c16d",
        PieChart: "b4e30332-a245-4a2e-9bf9-8d67bddef797",
        PolarChart: "fa549f53-7b44-4061-a9d2-bed477e8a235",
        BarChart: "4a966afb-e44f-4557-bd11-bed6c9c9c25d",
      },
      Ticket: [],
      FileName: "Reporte",
      ReportData: [],
      PageCreate: "/ExpressForm/FuncionalFormExpress",
      DataForm: [],
      DataFormSection: [],
      ListStatistic: [],
      DataFormSectionGrids: [],
      principalSchema: {},
      labels1: [],
      data1: [],
    };
  },
  props: ["FormId", "ReportQueryId", "Title", "ConfigId"],
  created() {
    if (this.ReportQueryId != null) {
      this.getReport();
    }
  },
  methods: {
    async getReport() {
      let data = JSON.stringify(this.principalSchema);
      let url = `Report/GetByReportQueryId?id=${this.ReportQueryId}&Data=${data}`;

      this.$axios
        .get(url)
        .then((response) => {
          if (response.data.Data == null) {
            this.ReportData = [];
          } else {
            this.ReportData = response.data.Data;

            const data = response.data.Data;

            data.forEach((result) => {
              this.labels1.push(result.Label);
              this.data1.push(result.Value);
            });
            this.labels1 = JSON.parse(JSON.stringify(this.labels1));
            this.data1 = JSON.parse(JSON.stringify(this.data1));
          }
        })
        .catch((error) => {
          // Manejo de errores
        });
    },
  },
  computed: {
    chartDataAvailable() {
      return this.labels1.length > 0;
    },
    selectedChart() {
      switch (this.ConfigId) {
        case this.ConfigSet.DonutChart:
          return "DonutChart";
        case this.ConfigSet.PieChart:
          return "PieChart";
        case this.ConfigSet.PolarChart:
          return "PolarChart";
        default:
          return "BarChart";
      }
    },
  },
};
</script>
