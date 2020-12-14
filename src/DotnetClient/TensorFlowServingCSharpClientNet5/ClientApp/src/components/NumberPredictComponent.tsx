import * as React from 'react';
import { RouteComponentProps } from 'react-router-dom';
import { connect } from 'react-redux';
import { ApplicationState } from '../store';
import * as NumberPredictStore from '../store/NumberPredict';
import { BarChartComponent } from './BarChart';
import { DrawComponent2 } from './DrawComponent2';

type NumberPredictProps =
    NumberPredictStore.NumberPredictState    
    & typeof NumberPredictStore.actionCreators
    & RouteComponentProps<any>;

class NumberPredictComponent extends React.PureComponent<NumberPredictProps> {
    
    public render() {

        return <div>
            <h1>MNIST Deep: number prediction</h1>
            <p>Please draw number and click "Predict" button.</p>
            <div className="row">
                <div className="col-sm-4">
                    <DrawComponent2 ref="drawControl" />
                    <div className="m-b-10 mt-3">
                        <button className="btn btn-primary m-r-10" onClick={() => {
                            this.props.tryPredictNumber((this.refs["drawControl"] as DrawComponent2).getImageData(), true);
                        }}>Predict Grpc</button>
                        <button className="btn btn-primary m-r-10" onClick={() => {
                            this.props.tryPredictNumber((this.refs["drawControl"] as DrawComponent2).getImageData(), false);
                        }}>Predict Rest</button>
                        <button className="btn btn-secondary" onClick={() => {
                            (this.refs["drawControl"] as DrawComponent2).clear();
                        }}>Clear</button>
                    </div>
                </div>
                <div className="col-sm-8">
                    {
                        this.props.loading ?
                            <p> Loading ... </p>
                            : this.props.loaded ?
                                (this.props.predictResult ?
                                    <div>
                                        <p><b>It is: {this.props.numberPredicted}</b></p>
                                        <BarChartComponent results={this.props.results} />
                                        <pre className="debug-region">{this.props.debugText}</pre>
                                    </div>
                                    : <p>
                                        <b>Error processing prediction: </b>
                                        <br />{this.props.errorMessage}
                                      </p>)
                            : null
                    }
                </div>
            </div>
        </div>;
    }
}

export default connect(
    (state: ApplicationState) => state.numberPredict, 
    NumberPredictStore.actionCreators                 
)(NumberPredictComponent as any);