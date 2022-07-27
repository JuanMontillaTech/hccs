<template>
    <div>
 
        <vueselect :options="list" :reduce="(row) => row.id" :label="field.sourceLabel" v-model="select"
        @input="setSelected" 
        >

        </vueselect>
    </div>
</template>

<script>


export default {
    name: 'InfiniteScroll',
    data: () => ({
        list: [],
        select: null,
        observer: null,
        limit: 10,
        search: '',
    }),
    props: [
         
        "field" 
    ],
  
    computed: {
        filtered() {
            //  return list.filter((country) => list.includes(this.search))
        },
        paginated() {
            // return this.filtered.slice(0, this.limit)
        },
        hasNextPage() {
            // return this.paginated.length < this.filtered.length
        },
    },
    mounted() {
        /**
         * You could do this directly in data(), but since these docs
         * are server side rendered, IntersectionObserver doesn't exist
         * in that environment, so we need to do it in mounted() instead.
         */
     
        this.$axios
            .get(`${this.field.sourceApi}`)
            .then((response) => {
                //  response.data.data.map((row) => {

                //   if(row.type === 1)

                //   this.getList(row.sourceApi,row.id )
                // });
                this.list = response.data.data;
            })
            .catch((error) => {
                this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
            });
        //this.observer = new IntersectionObserver(this.infiniteScroll)
    },
    methods: {
         setSelected(value) {          
            
            this.$emit( 'CustomChange',  this.field.field,value);           
        },
        
        async infiniteScroll([{ isIntersecting, target }]) {
            if (isIntersecting) {
                const ul = target.offsetParent
                const scrollTop = target.offsetParent.scrollTop
                this.limit += 10
                await this.$nextTick()
                ul.scrollTop = scrollTop
            }
        },
    },
}
</script>

<style scoped>
.loader {
    text-align: center;
    color: #bbbbbb;
}
</style>
