 
   <template>
<div>
<FormDynamicExpress> </FormDynamicExpress>

</div>
</template> 
 <script>
import { defineAsyncComponent } from 'vue';
export default {
  components: {
    FormDynamicExpress: defineAsyncComponent(() => import('./components/FormDynamicExpress.vue'))
  }
}
</script>
