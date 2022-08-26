<template>
<div>
<ListDynamicExpress> </ListDynamicExpress>

</div>
</template>
<script>
import { defineAsyncComponent } from 'vue';
export default {
  components: {
    ListDynamicExpress: defineAsyncComponent(() => import('./components/ListDynamicExpress.vue'))
  }
}
</script>

