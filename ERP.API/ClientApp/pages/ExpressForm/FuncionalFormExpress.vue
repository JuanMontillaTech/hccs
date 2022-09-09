 
   <template>
    <div>
    <FormMasterDynamic> </FormMasterDynamic>
    
    </div>
    </template> 
     <script>
    import { defineAsyncComponent } from 'vue';
    export default {
      components: {
        FormMasterDynamic: defineAsyncComponent(() => import('./components/FormMasterDynamic.vue'))
      }
    }
    </script>
    