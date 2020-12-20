import * as React from 'react';
import * as BarChart from 'react-chartjs-2';

type BarChartProps = {
    results: number[];
}

export class BarChartComponent extends React.Component<BarChartProps, {}> {
    public render() {

        return <BarChart.Bar
            data = {{
                labels: ['Cat', 'Dog'],
                datasets: [
                    {
                        data: this.props.results,
                        label: "Cats and dogs prediction dataset"
                    }
                ]
            }}
        />
    }
}