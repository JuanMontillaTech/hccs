<template>
    <div class="container">
        <div class="row" style="display: flex; justify-content: center; align-items: center; height: 100vh">
            <div class="col-6 my-5">
                <pie-chart :data="[['Blueberry', 44], ['Strawberry', 23], ['Mango', 12]]"></pie-chart>
            </div>
            <div class="col-6">
                <column-chart :data="[['Sun', 32], ['Mon', 46], ['Tue', 28], ['Wen', 18],['Fri', 38]]"></column-chart>
            </div>
            <div class="col-12 my-5">
                <line-chart :data="chartData" />
            </div>
        </div>
    </div>
</template>

<script>
 
export default {
      layout: 'default',
  
    data(){
        return {
            chartData: [["Jan", 4], ["Feb", 2], ["Mar", 10], ["Apr", 5], ["May", 3]]
        }
    }
}
</script>

<style>

</style>