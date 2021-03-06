
    // set the dimensions and margins of the graph
const margin = {top: 30, right: 30, bottom: 70, left: 60},
    width = 575 - margin.left - margin.right,
    height = 500 - margin.top - margin.bottom;

// append the svg object to the body of the page
const svg = d3.select("#my_dataviz")
  .append("svg")
    .attr("width", width + margin.left + margin.right)
    .attr("height", height + margin.top + margin.bottom)
  .append("g")
    .attr("transform", `translate(${margin.left},${margin.top})`);

// Parse the Data
d3.json("/Orders/BestSellerStoreJsonDetails").then( function(data) {

    // X axis
    const x = d3.scaleBand()
        .range([0, width])
        .domain(data.map(d => d.city))
        .padding(0.2);
    svg.append("g")
        .attr("transform", `translate(0, ${height})`)
        .call(d3.axisBottom(x))
        .selectAll("text")
        .attr("transform", "translate(-10,0)rotate(-45)")
        .style("text-anchor", "end");

    // Add Y axis
    const y = d3.scaleLinear()
        .domain([0, 5000])
        .range([height, 0]);
    svg.append("g")
        .call(d3.axisLeft(y));
    svg.append("g")
        .attr("class", "axis axis--y")
        .append("text")
        .attr("class", "axis-title")
        .attr("transform", "rotate(-90)")
        .attr("y", 6)
        .attr("dy", ".71em")
        .style("text-anchor", "end")
        .attr("fill", "#ffffff")
        .text("Total Income ₪");

    // Bars
    svg.selectAll("mybar")
        .data(data)
        .join("rect")
        .attr("x", d => x(d.city))
        .attr("width", x.bandwidth())
        .attr("fill", "#69b3a2")
        .attr("height", d => height - y(0))
        .attr("y", d => y(0))

    // Animation
    svg.selectAll("rect")
        .transition()
        .duration(2000)
        .attr("y", function (d) { return y(d.total); })
        .attr("height", function (d) { return height - y(d.total); })
        .delay(function (d, i) { console.log(i); return (i * 100) })

})
