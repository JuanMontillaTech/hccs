<template>
<div>

    <div class="container px-4">
        <div class="row">
            <div class="col-12">
                <h2 class="text-uppercase border-bottom border-secondary">dashboard</h2>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12 col-lg-12 mt-3">
                <div class="mb-5 border shadow p-3">
                    <line-chart :data="chartData" />
                </div>
            </div>
        </div>
        <div class="row gx-5">
            <div class="col-md-6 col-sm-12">
                <div class="mb-5 shadow p-3">
                    <pie-chart :data="[['Blueberry', 44], ['Strawberry', 23], ['Mango', 12]]"></pie-chart>
                </div>
            </div>
            <div class="col-md-6 col-sm-12">
                <div class="mb-5 shadow p-3">
                    <column-chart :data="[['Sun', 32], ['Mon', 46], ['Tue', 28], ['Wen', 18],['Fri', 38]]"></column-chart>
                </div>
            </div>
        </div>
    </div>
</div>
    
</template>

<script>
export default {
    layout: 'TheSlidebar',
    data(){
        return {
            chartData: [["Jan", 4], ["Feb", 2], ["Mar", 10], ["Apr", 5], ["May", 3]]
        }
    }
}
</script>

<style>

</style>