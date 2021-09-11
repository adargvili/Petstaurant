// set the dimensions and margins of the graph
const margin2 = { top: 30, right: 30, bottom: 80, left: 60 },
    width2 = 575 - margin2.left - margin2.right,
    height2 = 500 - margin2.top - margin2.bottom;

// append the svg object to the body of the page
const svg2 = d3.select("#my_dataviz2")
    .append("svg")
    .attr("width", width + margin2.left + margin2.right)
    .attr("height", height + margin2.top + margin2.bottom)
    .append("g")
    .attr("transform", `translate(${margin2.left},${margin2.top})`);

// Parse the Data
d3.json("/Orders/BestSellerDishJsonDetails").then(function (data) {

    // X axis
    const x = d3.scaleBand()
        .range([0, width])
        .domain(data.map(d => d.name))
        .padding(0.2);
    svg2.append("g")
        .attr("transform", `translate(0, ${height})`)
        .call(d3.axisBottom(x))
        .selectAll("text")
        .attr("transform", "translate(-10,0)rotate(-45)")
        .style("text-anchor", "end");

    // Add Y axis
    const y = d3.scaleLinear()
        .domain([0, 100])
        .range([height, 0]);
    svg2.append("g")
        .call(d3.axisLeft(y));
    svg2.append("g")
        .attr("class", "axis axis--y")
        .append("text")
        .attr("class", "axis-title")
        .attr("transform", "rotate(-90)")
        .attr("y", 6)
        .attr("dy", ".71em")
        .style("text-anchor", "end")
        .attr("fill", "#ffffff")
        .text("Total Number Of Orders");

    // Bars
    svg2.selectAll("mybar")
        .data(data)
        .join("rect")
        .attr("x", d => x(d.name))
        .attr("width", x.bandwidth())
        .attr("fill", "#69b3a2")
        .attr("height", d => height - y(0))
        .attr("y", d => y(0))

    // Animation
    svg2.selectAll("rect")
        .transition()
        .duration(2000)
        .attr("y", function (d) { return y(d.quantity); })
        .attr("height", function (d) { return height - y(d.quantity); })
        .delay(function (d, i) { console.log(i); return (i * 100) })

})