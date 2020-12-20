import * as React from 'react';
import * as BarChart from 'react-chartjs-2';

type BarChartProps = {
    results: number[];
}

export class BarChartComponent extends React.Component<BarChartProps, {}> {
    public render() {

        return <BarChart.Bar
            data = {{
                labels: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9],
                datasets: [
                    {
                        data: this.props.results,
                        label: "My First dataset"
                    }
                ]
            }}
        />
    }
}