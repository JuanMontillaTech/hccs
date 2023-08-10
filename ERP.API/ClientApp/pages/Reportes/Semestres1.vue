<script>
import {tsNullKeyword} from "@babel/types";

var numbro = require("numbro");
var moment = require("moment");

/**
 * Invoice Detail component
 */
export default {
  head() {
    return {
      title: `${this.title} `,
    };
  },
  data() {
    return {
      title: "Balance de comprobación ",
      id: null,
      Boxs: {Box1: 29567.82, Box2: 0, Box3: 0, Box4: 0, Box5: 0, Box6: 0, value: 0, Box7: 0},
      Balances2:  [
        {
          "Code": "6101",
          "Name": "Tempo y Culto",
          "Months": [
            {
              "month": 1,
              "value": 0.0
            },
            {
              "month": 2,
              "value": 0.0
            },
            {
              "month": 3,
              "value": 0.0
            },
            {
              "month": 4,
              "value": 0.0
            },
            {
              "month": 5,
              "value": 0.0
            },
            {
              "month": 6,
              "value": 0.0
            },
            {
              "month": 7,
              "value": 0.0
            }
          ]
        },
        {
          "Code": "6102",
          "Name": "Sueldo al Personal",
          "Months": [
            {
              "month": 1,
              "value": 4000.0
            },
            {
              "month": 2,
              "value": 4000.0
            },
            {
              "month": 3,
              "value": 22000.0
            },
            {
              "month": 4,
              "value": 4000.0
            },
            {
              "month": 5,
              "value": 5000.0
            },
            {
              "month": 6,
              "value": 5000.0
            },
            {
              "month": 7,
              "value": 44000.0
            }
          ]
        },
        {
          "Code": "6106",
          "Name": "CapacitaciÃ³n al Personal",
          "Months": [
            {
              "month": 1,
              "value": 0.0
            },
            {
              "month": 2,
              "value": 0.0
            },
            {
              "month": 3,
              "value": 0.0
            },
            {
              "month": 4,
              "value": 0.0
            },
            {
              "month": 5,
              "value": 0.0
            },
            {
              "month": 6,
              "value": 0.0
            },
            {
              "month": 7,
              "value": 0.0
            }
          ]
        },
        {
          "Code": "6107",
          "Name": "Cursillos y Retiros Espirituales",
          "Months": [
            {
              "month": 1,
              "value": 0.0
            },
            {
              "month": 2,
              "value": 0.0
            },
            {
              "month": 3,
              "value": 0.0
            },
            {
              "month": 4,
              "value": 0.0
            },
            {
              "month": 5,
              "value": 0.0
            },
            {
              "month": 6,
              "value": 20000.0
            },
            {
              "month": 7,
              "value": 20000.0
            }
          ]
        },
        {
          "Code": "6108",
          "Name": "Agua, Basura y Alcantarillado",
          "Months": [
            {
              "month": 1,
              "value": 0.0
            },
            {
              "month": 2,
              "value": 0.0
            },
            {
              "month": 3,
              "value": 0.0
            },
            {
              "month": 4,
              "value": 0.0
            },
            {
              "month": 5,
              "value": 0.0
            },
            {
              "month": 6,
              "value": 0.0
            },
            {
              "month": 7,
              "value": 0.0
            }
          ]
        },
        {
          "Code": "6109",
          "Name": "ElÃ©ctricidad",
          "Months": [
            {
              "month": 1,
              "value": 0.0
            },
            {
              "month": 2,
              "value": 0.0
            },
            {
              "month": 3,
              "value": 0.0
            },
            {
              "month": 4,
              "value": 0.0
            },
            {
              "month": 5,
              "value": 0.0
            },
            {
              "month": 6,
              "value": 0.0
            },
            {
              "month": 7,
              "value": 0.0
            }
          ]
        },
        {
          "Code": "6110",
          "Name": "TelÃ©fono y ComunicaciÃ³nes",
          "Months": [
            {
              "month": 1,
              "value": 5060.0
            },
            {
              "month": 2,
              "value": 5065.0
            },
            {
              "month": 3,
              "value": 5055.0
            },
            {
              "month": 4,
              "value": 5095.0
            },
            {
              "month": 5,
              "value": 5027.0
            },
            {
              "month": 6,
              "value": 5000.0
            },
            {
              "month": 7,
              "value": 30302.0
            }
          ]
        },
        {
          "Code": "6111",
          "Name": "Refrigerio",
          "Months": [
            {
              "month": 1,
              "value": 0.0
            },
            {
              "month": 2,
              "value": 0.0
            },
            {
              "month": 3,
              "value": 0.0
            },
            {
              "month": 4,
              "value": 0.0
            },
            {
              "month": 5,
              "value": 0.0
            },
            {
              "month": 6,
              "value": 0.0
            },
            {
              "month": 7,
              "value": 0.0
            }
          ]
        },
        {
          "Code": "6113",
          "Name": "SuscripciÃ³n y Compra de Libros",
          "Months": [
            {
              "month": 1,
              "value": 0.0
            },
            {
              "month": 2,
              "value": 0.0
            },
            {
              "month": 3,
              "value": 0.0
            },
            {
              "month": 4,
              "value": 0.0
            },
            {
              "month": 5,
              "value": 0.0
            },
            {
              "month": 6,
              "value": 0.0
            },
            {
              "month": 7,
              "value": 0.0
            }
          ]
        },
        {
          "Code": "6114",
          "Name": "Utiles de Limpieza",
          "Months": [
            {
              "month": 1,
              "value": 695.0
            },
            {
              "month": 2,
              "value": 512.0
            },
            {
              "month": 3,
              "value": 300.0
            },
            {
              "month": 4,
              "value": 769.0
            },
            {
              "month": 5,
              "value": 1641.0
            },
            {
              "month": 6,
              "value": 2439.0
            },
            {
              "month": 7,
              "value": 6356.0
            }
          ]
        },
        {
          "Code": "6115",
          "Name": "Mantenimiento Equipo de Transporte",
          "Months": [
            {
              "month": 1,
              "value": 0.0
            },
            {
              "month": 2,
              "value": 0.0
            },
            {
              "month": 3,
              "value": 0.0
            },
            {
              "month": 4,
              "value": 0.0
            },
            {
              "month": 5,
              "value": 0.0
            },
            {
              "month": 6,
              "value": 0.0
            },
            {
              "month": 7,
              "value": 0.0
            }
          ]
        },
        {
          "Code": "6116",
          "Name": "Combustibles y Lubricantes",
          "Months": [
            {
              "month": 1,
              "value": 5165.0
            },
            {
              "month": 2,
              "value": 10040.0
            },
            {
              "month": 3,
              "value": 10531.0
            },
            {
              "month": 4,
              "value": 3645.0
            },
            {
              "month": 5,
              "value": 4840.0
            },
            {
              "month": 6,
              "value": 9038.0
            },
            {
              "month": 7,
              "value": 43259.0
            }
          ]
        },
        {
          "Code": "6117",
          "Name": "Mantenimiento de Edificio y Rep.",
          "Months": [
            {
              "month": 1,
              "value": 0.0
            },
            {
              "month": 2,
              "value": 0.0
            },
            {
              "month": 3,
              "value": 0.0
            },
            {
              "month": 4,
              "value": 705.0
            },
            {
              "month": 5,
              "value": 0.0
            },
            {
              "month": 6,
              "value": 0.0
            },
            {
              "month": 7,
              "value": 705.0
            }
          ]
        },
        {
          "Code": "6118",
          "Name": "Viajes Nacionales y ViÃ¡ticos",
          "Months": [
            {
              "month": 1,
              "value": 0.0
            },
            {
              "month": 2,
              "value": 2865.0
            },
            {
              "month": 3,
              "value": 3147.0
            },
            {
              "month": 4,
              "value": 610.0
            },
            {
              "month": 5,
              "value": 2980.0
            },
            {
              "month": 6,
              "value": 2730.0
            },
            {
              "month": 7,
              "value": 12332.0
            }
          ]
        },
        {
          "Code": "6119",
          "Name": "Viajes al Extranjero ViÃ¡ticos",
          "Months": [
            {
              "month": 1,
              "value": 0.0
            },
            {
              "month": 2,
              "value": 0.0
            },
            {
              "month": 3,
              "value": 0.0
            },
            {
              "month": 4,
              "value": 0.0
            },
            {
              "month": 5,
              "value": 0.0
            },
            {
              "month": 6,
              "value": 0.0
            },
            {
              "month": 7,
              "value": 0.0
            }
          ]
        },
        {
          "Code": "6120",
          "Name": "Ayudas a Familiares",
          "Months": [
            {
              "month": 1,
              "value": 0.0
            },
            {
              "month": 2,
              "value": 0.0
            },
            {
              "month": 3,
              "value": 0.0
            },
            {
              "month": 4,
              "value": 0.0
            },
            {
              "month": 5,
              "value": 0.0
            },
            {
              "month": 6,
              "value": 0.0
            },
            {
              "month": 7,
              "value": 0.0
            }
          ]
        },
        {
          "Code": "6121",
          "Name": "Otras Ayudas",
          "Months": [
            {
              "month": 1,
              "value": 0.0
            },
            {
              "month": 2,
              "value": 2000.0
            },
            {
              "month": 3,
              "value": 2500.0
            },
            {
              "month": 4,
              "value": 5305.0
            },
            {
              "month": 5,
              "value": 7500.0
            },
            {
              "month": 6,
              "value": 4720.0
            },
            {
              "month": 7,
              "value": 22025.0
            }
          ]
        },
        {
          "Code": "6122",
          "Name": "Regalos y Fiestas",
          "Months": [
            {
              "month": 1,
              "value": 2000.0
            },
            {
              "month": 2,
              "value": 1800.0
            },
            {
              "month": 3,
              "value": 0.0
            },
            {
              "month": 4,
              "value": 3000.0
            },
            {
              "month": 5,
              "value": 22000.0
            },
            {
              "month": 6,
              "value": 0.0
            },
            {
              "month": 7,
              "value": 28800.0
            }
          ]
        },
        {
          "Code": "6123",
          "Name": "Obras Apostolado y FundaciÃ³n",
          "Months": [
            {
              "month": 1,
              "value": 0.0
            },
            {
              "month": 2,
              "value": 0.0
            },
            {
              "month": 3,
              "value": 0.0
            },
            {
              "month": 4,
              "value": 0.0
            },
            {
              "month": 5,
              "value": 0.0
            },
            {
              "month": 6,
              "value": 0.0
            },
            {
              "month": 7,
              "value": 0.0
            }
          ]
        },
        {
          "Code": "6124",
          "Name": "MÃ©dicos y Medicinas",
          "Months": [
            {
              "month": 1,
              "value": 5728.0
            },
            {
              "month": 2,
              "value": 12089.0
            },
            {
              "month": 3,
              "value": 40812.0
            },
            {
              "month": 4,
              "value": 2415.0
            },
            {
              "month": 5,
              "value": 16326.0
            },
            {
              "month": 6,
              "value": 111702.0
            },
            {
              "month": 7,
              "value": 189072.0
            }
          ]
        },
        {
          "Code": "6125",
          "Name": "Mant. de Equipos ElÃ©ctricos y Tecnol.",
          "Months": [
            {
              "month": 1,
              "value": 0.0
            },
            {
              "month": 2,
              "value": 0.0
            },
            {
              "month": 3,
              "value": 0.0
            },
            {
              "month": 4,
              "value": 0.0
            },
            {
              "month": 5,
              "value": 0.0
            },
            {
              "month": 6,
              "value": 0.0
            },
            {
              "month": 7,
              "value": 0.0
            }
          ]
        },
        {
          "Code": "6126",
          "Name": "Mant. Muebles y Equipos de Oficina",
          "Months": [
            {
              "month": 1,
              "value": 0.0
            },
            {
              "month": 2,
              "value": 0.0
            },
            {
              "month": 3,
              "value": 0.0
            },
            {
              "month": 4,
              "value": 0.0
            },
            {
              "month": 5,
              "value": 0.0
            },
            {
              "month": 6,
              "value": 0.0
            },
            {
              "month": 7,
              "value": 0.0
            }
          ]
        },
        {
          "Code": "6127",
          "Name": "Gastos Financieros",
          "Months": [
            {
              "month": 1,
              "value": 0.0
            },
            {
              "month": 2,
              "value": 0.0
            },
            {
              "month": 3,
              "value": 0.0
            },
            {
              "month": 4,
              "value": 0.0
            },
            {
              "month": 5,
              "value": 0.0
            },
            {
              "month": 6,
              "value": 0.0
            },
            {
              "month": 7,
              "value": 0.0
            }
          ]
        },
        {
          "Code": "6128",
          "Name": "FormaciÃ³n (Noviciado y Juniorado)",
          "Months": [
            {
              "month": 1,
              "value": 0.0
            },
            {
              "month": 2,
              "value": 0.0
            },
            {
              "month": 3,
              "value": 0.0
            },
            {
              "month": 4,
              "value": 0.0
            },
            {
              "month": 5,
              "value": 0.0
            },
            {
              "month": 6,
              "value": 0.0
            },
            {
              "month": 7,
              "value": 0.0
            }
          ]
        },
        {
          "Code": "6129",
          "Name": "Materiales de Oficina",
          "Months": [
            {
              "month": 1,
              "value": 0.0
            },
            {
              "month": 2,
              "value": 0.0
            },
            {
              "month": 3,
              "value": 1000.0
            },
            {
              "month": 4,
              "value": 0.0
            },
            {
              "month": 5,
              "value": 0.0
            },
            {
              "month": 6,
              "value": 0.0
            },
            {
              "month": 7,
              "value": 1000.0
            }
          ]
        },
        {
          "Code": "6130",
          "Name": "Impuestos",
          "Months": [
            {
              "month": 1,
              "value": 0.0
            },
            {
              "month": 2,
              "value": 0.0
            },
            {
              "month": 3,
              "value": 0.0
            },
            {
              "month": 4,
              "value": 0.0
            },
            {
              "month": 5,
              "value": 0.0
            },
            {
              "month": 6,
              "value": 0.0
            },
            {
              "month": 7,
              "value": 0.0
            }
          ]
        },
        {
          "Code": "6131",
          "Name": "AlimentaciÃ³n",
          "Months": [
            {
              "month": 1,
              "value": 27828.0
            },
            {
              "month": 2,
              "value": 22912.0
            },
            {
              "month": 3,
              "value": 24017.0
            },
            {
              "month": 4,
              "value": 20110.0
            },
            {
              "month": 5,
              "value": 24736.0
            },
            {
              "month": 6,
              "value": 18172.0
            },
            {
              "month": 7,
              "value": 137775.0
            }
          ]
        },
        {
          "Code": "6132",
          "Name": "DonaciÃ³n y Obsequios",
          "Months": [
            {
              "month": 1,
              "value": 0.0
            },
            {
              "month": 2,
              "value": 0.0
            },
            {
              "month": 3,
              "value": 0.0
            },
            {
              "month": 4,
              "value": 0.0
            },
            {
              "month": 5,
              "value": 0.0
            },
            {
              "month": 6,
              "value": 0.0
            },
            {
              "month": 7,
              "value": 0.0
            }
          ]
        },
        {
          "Code": "6134",
          "Name": "Embellecimiento y JardinerÃ­a",
          "Months": [
            {
              "month": 1,
              "value": 0.0
            },
            {
              "month": 2,
              "value": 0.0
            },
            {
              "month": 3,
              "value": 0.0
            },
            {
              "month": 4,
              "value": 0.0
            },
            {
              "month": 5,
              "value": 0.0
            },
            {
              "month": 6,
              "value": 0.0
            },
            {
              "month": 7,
              "value": 0.0
            }
          ]
        },
        {
          "Code": "6136",
          "Name": "Vestidos",
          "Months": [
            {
              "month": 1,
              "value": 1200.0
            },
            {
              "month": 2,
              "value": 3107.0
            },
            {
              "month": 3,
              "value": 650.0
            },
            {
              "month": 4,
              "value": 2990.0
            },
            {
              "month": 5,
              "value": 1300.0
            },
            {
              "month": 6,
              "value": 0.0
            },
            {
              "month": 7,
              "value": 9247.0
            }
          ]
        },
        {
          "Code": "6137",
          "Name": "Otros Gastos (Utensilios)",
          "Months": [
            {
              "month": 1,
              "value": 7245.0
            },
            {
              "month": 2,
              "value": 15199.0
            },
            {
              "month": 3,
              "value": 7805.0
            },
            {
              "month": 4,
              "value": 12292.0
            },
            {
              "month": 5,
              "value": 6867.0
            },
            {
              "month": 6,
              "value": 7113.0
            },
            {
              "month": 7,
              "value": 56521.0
            }
          ]
        },
        {
          "Code": "6138",
          "Name": "Vacaciones y Diversiones",
          "Months": [
            {
              "month": 1,
              "value": 0.0
            },
            {
              "month": 2,
              "value": 0.0
            },
            {
              "month": 3,
              "value": 0.0
            },
            {
              "month": 4,
              "value": 0.0
            },
            {
              "month": 5,
              "value": 0.0
            },
            {
              "month": 6,
              "value": 0.0
            },
            {
              "month": 7,
              "value": 0.0
            }
          ]
        },
        {
          "Code": "6139",
          "Name": "Utiles de Aseo Personal ",
          "Months": [
            {
              "month": 1,
              "value": 0.0
            },
            {
              "month": 2,
              "value": 0.0
            },
            {
              "month": 3,
              "value": 0.0
            },
            {
              "month": 4,
              "value": 0.0
            },
            {
              "month": 5,
              "value": 0.0
            },
            {
              "month": 6,
              "value": 0.0
            },
            {
              "month": 7,
              "value": 0.0
            }
          ]
        },
        {
          "Code": "6143",
          "Name": "ObtenciÃ³n y LegalizaciÃ³n D.",
          "Months": [
            {
              "month": 1,
              "value": 0.0
            },
            {
              "month": 2,
              "value": 0.0
            },
            {
              "month": 3,
              "value": 0.0
            },
            {
              "month": 4,
              "value": 0.0
            },
            {
              "month": 5,
              "value": 0.0
            },
            {
              "month": 6,
              "value": 0.0
            },
            {
              "month": 7,
              "value": 0.0
            }
          ]
        },
        {
          "Code": "6144",
          "Name": "10% Caja General",
          "Months": [
            {
              "month": 1,
              "value": 3700.0
            },
            {
              "month": 2,
              "value": 3700.0
            },
            {
              "month": 3,
              "value": 3700.0
            },
            {
              "month": 4,
              "value": 3700.0
            },
            {
              "month": 5,
              "value": 3700.0
            },
            {
              "month": 6,
              "value": 3700.0
            },
            {
              "month": 7,
              "value": 22200.0
            }
          ]
        },
        {
          "Code": "6147",
          "Name": "Pro-Casa Hermanas Mayores",
          "Months": [
            {
              "month": 1,
              "value": 0.0
            },
            {
              "month": 2,
              "value": 0.0
            },
            {
              "month": 3,
              "value": 0.0
            },
            {
              "month": 4,
              "value": 0.0
            },
            {
              "month": 5,
              "value": 0.0
            },
            {
              "month": 6,
              "value": 0.0
            },
            {
              "month": 7,
              "value": 0.0
            }
          ]
        },
        {
          "Code": "6149",
          "Name": "Gastos Funebre",
          "Months": [
            {
              "month": 1,
              "value": 0.0
            },
            {
              "month": 2,
              "value": 0.0
            },
            {
              "month": 3,
              "value": 0.0
            },
            {
              "month": 4,
              "value": 0.0
            },
            {
              "month": 5,
              "value": 0.0
            },
            {
              "month": 6,
              "value": 0.0
            },
            {
              "month": 7,
              "value": 0.0
            }
          ]
        },
        {
          "Code": "6150",
          "Name": "Publicidad y Propaganda",
          "Months": [
            {
              "month": 1,
              "value": 0.0
            },
            {
              "month": 2,
              "value": 0.0
            },
            {
              "month": 3,
              "value": 0.0
            },
            {
              "month": 4,
              "value": 0.0
            },
            {
              "month": 5,
              "value": 0.0
            },
            {
              "month": 6,
              "value": 0.0
            },
            {
              "month": 7,
              "value": 0.0
            }
          ]
        },
        {
          "Code": "6152",
          "Name": "Sistema de CÃ³mputo",
          "Months": [
            {
              "month": 1,
              "value": 0.0
            },
            {
              "month": 2,
              "value": 0.0
            },
            {
              "month": 3,
              "value": 0.0
            },
            {
              "month": 4,
              "value": 0.0
            },
            {
              "month": 5,
              "value": 0.0
            },
            {
              "month": 6,
              "value": 0.0
            },
            {
              "month": 7,
              "value": 0.0
            }
          ]
        },
        {
          "Code": "6153",
          "Name": "Comida Animales",
          "Months": [
            {
              "month": 1,
              "value": 0.0
            },
            {
              "month": 2,
              "value": 0.0
            },
            {
              "month": 3,
              "value": 0.0
            },
            {
              "month": 4,
              "value": 0.0
            },
            {
              "month": 5,
              "value": 0.0
            },
            {
              "month": 6,
              "value": 0.0
            },
            {
              "month": 7,
              "value": 0.0
            }
          ]
        },
        {
          "Code": "6166",
          "Name": "Cargos Bancario",
          "Months": [
            {
              "month": 1,
              "value": 214.0
            },
            {
              "month": 2,
              "value": 203.0
            },
            {
              "month": 3,
              "value": 214.0
            },
            {
              "month": 4,
              "value": 183.0
            },
            {
              "month": 5,
              "value": 100.0
            },
            {
              "month": 6,
              "value": 0.0
            },
            {
              "month": 7,
              "value": 913.0
            }
          ]
        },
        {
          "Code": "",
          "Name": "InstalaciÃ³n de Sistema Solar ",
          "Months": [
            {
              "month": 1,
              "value": 0.0
            },
            {
              "month": 2,
              "value": 0.0
            },
            {
              "month": 3,
              "value": 0.0
            },
            {
              "month": 4,
              "value": 0.0
            },
            {
              "month": 5,
              "value": 0.0
            },
            {
              "month": 6,
              "value": 0.0
            },
            {
              "month": 7,
              "value": 0.0
            }
          ]
        },
        {
          "Code": "",
          "Name": "Seguridad Social AFP.",
          "Months": [
            {
              "month": 1,
              "value": 0.0
            },
            {
              "month": 2,
              "value": 0.0
            },
            {
              "month": 3,
              "value": 0.0
            },
            {
              "month": 4,
              "value": 0.0
            },
            {
              "month": 5,
              "value": 0.0
            },
            {
              "month": 6,
              "value": 0.0
            },
            {
              "month": 7,
              "value": 0.0
            }
          ]
        },
        {
          "Code": "",
          "Name": "Proyecto Adveniat",
          "Months": [
            {
              "month": 1,
              "value": 0.0
            },
            {
              "month": 2,
              "value": 0.0
            },
            {
              "month": 3,
              "value": 0.0
            },
            {
              "month": 4,
              "value": 0.0
            },
            {
              "month": 5,
              "value": 0.0
            },
            {
              "month": 6,
              "value": 0.0
            },
            {
              "month": 7,
              "value": 0.0
            }
          ]
        },
        {
          "Code": "110",
          "Name": "Informe Adicional -Ahorro",
          "Months": [
            {
              "month": 1,
              "value": 1135.0
            },
            {
              "month": 2,
              "value": 1026.0
            },
            {
              "month": 3,
              "value": 1136.0
            },
            {
              "month": 4,
              "value": 130000.0
            },
            {
              "month": 5,
              "value": 0.0
            },
            {
              "month": 6,
              "value": 0.0
            },
            {
              "month": 7,
              "value": 133297.0
            }
          ]
        }
      ],
      Balances: [
        {
          Code: "4110",
          Name: "Sueldo por Trabajo Hermanas",
          Months: [
            {month: 1, value: 66215},
            {month: 2, value: 66215},
            {month: 3, value: 66215},
            {month: 4, value: 66215},
            {month: 5, value: 65000},
            {month: 6, value: 66215},
          ],
          TotalMoth: 396075
        },
        {
          Code: "4201",
          Name: "Intereses Bancarios",
          Months: [
            {month: 1, value: 1135.07},
            {month: 2, value: 1025.65},
            {month: 3, value: 1135.97},
            {month: 4, value: 824.84},
            {month: 5, value: 0},
            {month: 6, value: 0},
          ],
          TotalMoth: 4121.53
        },
        {
          Code: "4202",
          Name: "Donativos",
          Months: [
            {month: 1, value: 0},
            {month: 2, value: 0},
            {month: 3, value: 0},
            {month: 4, value: 0},
            {month: 5, value: 0},
            {month: 6, value: 0},
          ],
          TotalMoth: 0
        },
        {
          Code: "4203",
          Name: "Subsidios ",
          Months: [
            {month: 1, value: 0},
            {month: 2, value: 0},
            {month: 3, value: 0},
            {month: 4, value: 0},
            {month: 5, value: 0},
            {month: 6, value: 0},
          ],
          TotalMoth: 0
        },
        {
          Code: "",
          Name: "Minerd",
          Months: [
            {month: 1, value: 0},
            {month: 2, value: 0},
            {month: 3, value: 0},
            {month: 4, value: 130000},
            {month: 5, value: 0},
            {month: 6, value: 0},
          ],
          TotalMoth: 130000
        },
        {
          Code: "10",
          Name: "Informe Adicional -Retiro Ahorro",
          Months: [
            {month: 1, value: 213.66},
            {month: 2, value: 202.72},
            {month: 3, value: 213.75},
            {month: 4, value: 50182.63},
            {month: 5, value: 100.15},
            {month: 6, value: 0},
          ],
          TotalMoth: 50912.91
        },

      ],

      ListTotal: [],
      ListBalanceACT1: [],
      ListBalanceACT2: [],
      ListBalanceACT3: [],
      ListBalanceACT4: [],
      ListBalanceACT5: [],
      ListBalanceACT6: [],
      items: [
        {
          text: "Reporte",
        },
      ],
      company: {},
    };
  },
  created() {


  },
  methods: {
    getMonthValue(monthNumber, Months) {
      const monthData = Months.find(month => month.month === monthNumber);
      if (monthData) {
        return monthData.value;
      } else {
        return 0.0;
      }
    },
    getTotalForMonth(Month, Balances) {
      let totalMonth1 = 0;

      for (const balance of Balances) {

        totalMonth1 += this.getMonthValue(Month, balance.Months);

      }

      return totalMonth1;
    },

    SetTotal(globalTotal) {
      return numbro(globalTotal).format("0,0.00");
    },
    TotalList(type) {
      var dataFillter = this.ListBalanceACT1.filter((row) => row.origen === type);
      this.totalbalance = dataFillter.reduce(function (sum, row) {
        let lineTotal = parseFloat(row.balance);
        if (!isNaN(lineTotal)) {
          return sum + lineTotal;
        }
      }, 0);

      return this.totalbalance;
    },
    TotalList1(type) {
      var dataFillter = this.ListBalanceACT1.filter((row) => row.origen === type);
      this.totalbalance = dataFillter.reduce(function (sum, row) {
        let lineTotal = parseFloat(row.balance);
        if (!isNaN(lineTotal)) {
          return sum + lineTotal;
        }
      }, 0);

      return this.totalbalance;
    },
    TotalList2(type) {
      var dataFillter = this.ListBalanceACT2.filter((row) => row.origen === type);
      this.totalbalance = dataFillter.reduce(function (sum, row) {
        let lineTotal = parseFloat(row.balance);
        if (!isNaN(lineTotal)) {
          return sum + lineTotal;
        }
      }, 0);

      return this.totalbalance;
    },
    TotalList3(type) {
      var dataFillter = this.ListBalanceACT3.filter((row) => row.origen === type);
      this.totalbalance = dataFillter.reduce(function (sum, row) {
        let lineTotal = parseFloat(row.balance);
        if (!isNaN(lineTotal)) {
          return sum + lineTotal;
        }
      }, 0);

      return this.totalbalance;
    },
    TotalList4(type) {
      var dataFillter = this.ListBalanceACT4.filter((row) => row.origen === type);
      this.totalbalance = dataFillter.reduce(function (sum, row) {
        let lineTotal = parseFloat(row.balance);
        if (!isNaN(lineTotal)) {
          return sum + lineTotal;
        }
      }, 0);

      return this.totalbalance;
    },
    TotalList5(type) {
      var dataFillter = this.ListBalanceACT5.filter((row) => row.origen === type);
      this.totalbalance = dataFillter.reduce(function (sum, row) {
        let lineTotal = parseFloat(row.balance);
        if (!isNaN(lineTotal)) {
          return sum + lineTotal;
        }
      }, 0);

      return this.totalbalance;
    },
    TotalList6(type) {
      var dataFillter = this.ListBalanceACT6.filter((row) => row.origen === type);
      this.totalbalance = dataFillter.reduce(function (sum, row) {
        let lineTotal = parseFloat(row.balance);
        if (!isNaN(lineTotal)) {
          return sum + lineTotal;
        }
      }, 0);

      return this.totalbalance;
    },
    LoadData1() {
      let url =
        `Journal/GetAllLedgerAccountByCodeMonth?Code=EST&Month=1`;
      this.$axios
        .get(url, {
          headers: {
            "Content-Type": "application/json",
          },
        })
        .then((response) => {
          this.ListBalanceACT1 = response.data.data;
        })
        .catch((error) => {

        });
    },
    LoadData2() {
      let url =
        `Journal/GetAllLedgerAccountByCodeMonth?Code=EST&Month=2`;
      this.$axios
        .get(url, {
          headers: {
            "Content-Type": "application/json",
          },
        })
        .then((response) => {
          this.ListBalanceACT2 = response.data.data;
        })
        .catch((error) => {

        });
    },
    LoadData3() {
      let url =
        `Journal/GetAllLedgerAccountByCodeMonth?Code=EST&Month=3`;
      this.$axios
        .get(url, {
          headers: {
            "Content-Type": "application/json",
          },
        })
        .then((response) => {
          this.ListBalanceACT3 = response.data.data;
        })
        .catch((error) => {

        });
    },
    LoadData4() {
      let url =
        `Journal/GetAllLedgerAccountByCodeMonth?Code=EST&Month=4`;
      this.$axios
        .get(url, {
          headers: {
            "Content-Type": "application/json",
          },
        })
        .then((response) => {
          this.ListBalanceACT4 = response.data.data;
        })
        .catch((error) => {

        });
    },
    LoadData5() {
      let url =
        `Journal/GetAllLedgerAccountByCodeMonth?Code=EST&Month=5`;
      this.$axios
        .get(url, {
          headers: {
            "Content-Type": "application/json",
          },
        })
        .then((response) => {
          this.ListBalanceACT5 = response.data.data;
        })
        .catch((error) => {

        });
    },
    LoadData6() {
      let url =
        `Journal/GetAllLedgerAccountByCodeMonth?Code=EST&Month=6`;
      this.$axios
        .get(url, {
          headers: {
            "Content-Type": "application/json",
          },
        })
        .then((response) => {
          this.ListBalanceACT6 = response.data.data;
        })
        .catch((error) => {

        });
    },
  },
};
</script>

<template>
  <div>
    <PageHeader :items="items"/>

    <div class="row">
      <div class="col-lg-12">
        <div class="card">
          <div class="card-body">
            <CompanyHead title="Estado de resultado"></CompanyHead>
            <hr class="my-4"/>

            <div class="py-2">
              <div class="table-responsive">
                <table class="table">
                  <tbody>
                  <tr>
                    <th>Nombre de Cuenta</th>
                    <td>

                    </td>
                    <th>ENERO</th>
                    <th>FEBRERO</th>
                    <th>MARZO</th>
                    <th>ABRIL</th>
                    <TH> MAYO</TH>
                    <th>JUNIO</th>
                    <th>Total de Gastos</th>
                  </tr>
                  <tr>
                    <th>Caja Disponible actual</th>

                    <td>

                    </td>
                    <td>
                      {{ Boxs.Box1 }}
                    </td>
                    <td>
                      {{ Boxs.Box2 }}
                    </td>
                    <td>
                      {{ Boxs.Box3 }}
                    </td>
                    <td>
                      {{ Boxs.Box4 }}
                    </td>
                    <td>
                      {{ Boxs.Box5 }}
                    </td>
                    <td>
                      {{ Boxs.Box6 }}
                    </td>
                    <td>
                      {{ Boxs.Box7 }}
                    </td>
                  </tr>
                  <tr>
                    <th>
                      <h4>4 </h4>
                    </th>
                    <th>
                      <h4> INGRESOS </h4>
                    </th>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>

                  </tr>
                  <tr v-for="row in Balances" :key="row.Code">
                    <Td>
                      {{ row.Code }}
                    </Td>
                    <td>
                      {{ row.Name }}
                    </td>
                    <td>
                      {{ SetTotal(getMonthValue(1, row.Months)) }}
                    </td>
                    <td>
                      {{ SetTotal(getMonthValue(2, row.Months)) }}
                    </td>
                    <td>
                      {{ SetTotal(getMonthValue(3, row.Months)) }}
                    </td>
                    <td>
                      {{ SetTotal(getMonthValue(4, row.Months)) }}
                    </td>
                    <td>
                      {{ SetTotal(getMonthValue(5, row.Months)) }}
                    </td>
                    <td>
                      {{ SetTotal(getMonthValue(6, row.Months)) }}
                    </td>
                    <td>
                      {{ row.TotalMoth }}
                    </td>
                  </tr>

                  </tbody>
                  <tfoot>
                  <tr>
                    <Th>
                      Total de Ingresos del Mes
                    </Th>
                    <Th>

                    </Th>
                    <td>
                      {{ SetTotal(getTotalForMonth(1,Balances)) }}
                    </td>
                    <td>
                      {{ SetTotal(getTotalForMonth(2,Balances)) }}
                    </td>
                    <td>
                      {{ SetTotal(getTotalForMonth(3,Balances)) }}
                    </td>
                    <td>
                      {{ SetTotal(getTotalForMonth(4,Balances)) }}
                    </td>
                    <td>
                      {{ SetTotal(getTotalForMonth(5,Balances)) }}
                    </td>
                    <td>
                      {{ SetTotal(getTotalForMonth(6,Balances)) }}
                    </td>
                    <td>

                      {{ SetTotal(getTotalForMonth(1,Balances) + getTotalForMonth(2,Balances) + getTotalForMonth(3,Balances) + getTotalForMonth(4,Balances) + getTotalForMonth(5,Balances) + getTotalForMonth(6,Balances)) }}
                    </td>
                  </tr>
                  <tr>
                    <Th>
                      Total de Ingresos + Caja
                    </Th>
                    <Th>

                    </Th>
                    <td>
                      {{ SetTotal(getTotalForMonth(1,Balances) + Boxs.Box1) }}
                    </td>
                    <td>
                      {{ SetTotal(getTotalForMonth(2,Balances) + Boxs.Box2) }}
                    </td>
                    <td>
                      {{ SetTotal(getTotalForMonth(3,Balances) + Boxs.Box3) }}
                    </td>
                    <td>
                      {{ SetTotal(getTotalForMonth(4,Balances) + Boxs.Box4) }}
                    </td>
                    <td>
                      {{ SetTotal(getTotalForMonth(5,Balances) + Boxs.Box5) }}
                    </td>
                    <td>
                      {{ SetTotal(getTotalForMonth(6,Balances) + Boxs.Box6) }}
                    </td>
                    <td>

                      {{ SetTotal(getTotalForMonth(1,Balances) + getTotalForMonth(2,Balances) + getTotalForMonth(3,Balances) + getTotalForMonth(4,Balances) + getTotalForMonth(5,Balances) + getTotalForMonth(6,Balances)) }}
                    </td>
                  </tr>
                  </tfoot>
                </table>


                <table class="table">
                  <tbody>

                  <tr>
                    <th>
                      <h4>61</h4>
                    </th>
                    <th>
                      <h4> EGRESOS / SALIDAS </h4>
                    </th>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>

                  </tr>
                  <tr v-for="row in Balances2" :key="row.Code">
                    <Td>
                      {{ row.Code }}
                    </Td>
                    <td>
                      {{ row.Name }}
                    </td>
                    <td>
                      {{ SetTotal(getMonthValue(1, row.Months)) }}
                    </td>
                    <td>
                      {{ SetTotal(getMonthValue(2, row.Months)) }}
                    </td>
                    <td>
                      {{ SetTotal(getMonthValue(3, row.Months)) }}
                    </td>
                    <td>
                      {{ SetTotal(getMonthValue(4, row.Months)) }}
                    </td>
                    <td>
                      {{ SetTotal(getMonthValue(5, row.Months)) }}
                    </td>
                    <td>
                      {{ SetTotal(getMonthValue(6, row.Months)) }}
                    </td>
                    <td>
                      {{ row.TotalMoth }}
                    </td>
                  </tr>

                  </tbody>
                  <tfoot>
                  <tr>
                    <Th>
                      Total de Egresos
                    </Th>
                    <Th>

                    </Th>
                    <td>
                      {{ SetTotal(getTotalForMonth(1 ,Balances2)) }}
                    </td>
                    <td>
                      {{ SetTotal(getTotalForMonth(2,Balances2)) }}
                    </td>
                    <td>
                      {{ SetTotal(getTotalForMonth(3,Balances2)) }}
                    </td>
                    <td>
                      {{ SetTotal(getTotalForMonth(4,Balances2)) }}
                    </td>
                    <td>
                      {{ SetTotal(getTotalForMonth(5,Balances2)) }}
                    </td>
                    <td>
                      {{ SetTotal(getTotalForMonth(6,Balances2)) }}
                    </td>
                    <td>

                      {{ SetTotal(getTotalForMonth(1,Balances2) + getTotalForMonth(2,Balances2) + getTotalForMonth(3,Balances2) + getTotalForMonth(4,Balances2) + getTotalForMonth(5,Balances2) + getTotalForMonth(6,Balances2)) }}
                    </td>
                  </tr>
                  <tr>
                    <Th>
                      Total de Ingresos
                    </Th>
                    <Th>

                    </Th>
                    <td>
                      {{ SetTotal(getTotalForMonth(1,Balances) + Boxs.Box1) }}
                    </td>
                    <td>
                      {{ SetTotal(getTotalForMonth(2,Balances) + Boxs.Box2) }}
                    </td>
                    <td>
                      {{ SetTotal(getTotalForMonth(3,Balances) + Boxs.Box3) }}
                    </td>
                    <td>
                      {{ SetTotal(getTotalForMonth(4,Balances) + Boxs.Box4) }}
                    </td>
                    <td>
                      {{ SetTotal(getTotalForMonth(5,Balances) + Boxs.Box5) }}
                    </td>
                    <td>
                      {{ SetTotal(getTotalForMonth(6,Balances) + Boxs.Box6) }}
                    </td>
                    <td>

                      {{ SetTotal(getTotalForMonth(1,Balances) + getTotalForMonth(2,Balances) + getTotalForMonth(3,Balances) + getTotalForMonth(4,Balances) + getTotalForMonth(5,Balances) + getTotalForMonth(6,Balances)) }}
                    </td>
                  </tr>
                  <tr>
                    <Th>
                      Caja Disponible o Diferencia
                    </Th>
                    <Th>

                    </Th>
                    <td>
                      {{ SetTotal(getTotalForMonth(1,Balances) + Boxs.Box1)  }}
                    </td>
                    <td>
                      {{ SetTotal(getTotalForMonth(2,Balances) + Boxs.Box2) }}
                    </td>
                    <td>
                      {{ SetTotal(getTotalForMonth(3,Balances) + Boxs.Box3) }}
                    </td>
                    <td>
                      {{ SetTotal(getTotalForMonth(4,Balances) + Boxs.Box4) }}
                    </td>
                    <td>
                      {{ SetTotal(getTotalForMonth(5,Balances) + Boxs.Box5) }}
                    </td>
                    <td>
                      {{ SetTotal(getTotalForMonth(6,Balances) + Boxs.Box6) }}
                    </td>
                    <td>

                      {{ SetTotal(getTotalForMonth(1,Balances) + getTotalForMonth(2,Balances) + getTotalForMonth(3,Balances) + getTotalForMonth(4,Balances) + getTotalForMonth(5,Balances) + getTotalForMonth(6,Balances)) }}
                    </td>
                  </tr>
                  </tfoot>
                </table>

              </div>
              <div class="d-print-none mt-4">
                <print></print>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
    <!-- end row -->
  </div>
</template>
<style>
.doubleLine {
  border-top: double;
}
</style>
