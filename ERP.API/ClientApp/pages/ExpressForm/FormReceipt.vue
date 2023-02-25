
   <template>
    <div>
    <Receipt> </Receipt>

    </div>
    </template>
     <script>
    import { defineAsyncComponent } from 'vue';
    export default {
      components: {
        Receipt: defineAsyncComponent(() => import('./components/Receipt.vue'))
      }
    }
    </script>
