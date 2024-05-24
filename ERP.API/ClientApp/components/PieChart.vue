<script>
import { Pie } from "vue-chartjs";
import { Colerslist } from "../helpers/colors4";
export default {
  extends: Pie,
  props: {
    labels: {
      type: Array,
      required: true,
    },
    dataDashboard: {
      type: Array,
      required: true,
    },
  },
  mounted() {
    this.renderChart(
      {
        labels: this.labels,
        datasets: [
          {
            data: this.dataDashboard,
            backgroundColor: Colerslist,
            hoverBackgroundColor: Colerslist,
            hoverBorderColor: "#fff",
          },
        ],
      },
      {
        maintainAspectRatio: true,
        responsive: true,
      }
    );
  },
};
</script>
