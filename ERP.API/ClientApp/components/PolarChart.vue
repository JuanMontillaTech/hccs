<script>
import { PolarArea } from "vue-chartjs";
import { Colerslist } from "../helpers/colors";
export default {
  extends: PolarArea,
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
            barPercentage: 0.4,
            label: this.titleHeader,
            borderColor: this.bgColor,
            borderWidth: 1,
            hoverBorderColor: this.bgHoveColor,
            data: this.dataDashboard,
            backgroundColor: Colerslist,
            hoverBackgroundColor: Colerslist,
          },
        ],
      },
      {
        maintainAspectRatio: true,
        legend: {
          position: "top",
        },
      }
    );
  },
};
</script>
