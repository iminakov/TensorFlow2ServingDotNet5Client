import * as React from 'react';
import { RouteComponentProps } from 'react-router-dom';
import { connect } from 'react-redux';
import { ApplicationState } from '../../store';
import * as CatsAndDogsPredictStore from '../../store/CatsAndDogsPredict';
import { BarChartComponent } from './BarChart';
import SimpleImageUpload from './SimpleImageUpload';

type CatsAndDogsPredictProps =
CatsAndDogsPredictStore.CatsAndDogsPredictState    
    & typeof CatsAndDogsPredictStore.actionCreators
    & RouteComponentProps<any>;

class CatsAndDogsPredictComponent extends React.PureComponent<CatsAndDogsPredictProps> {
    
    public render() {

        return <div>
            <h1>Cats and Dogs prediction</h1>
            <p>Please select picture and click "Predict" button.</p>
            <div className="row">
                <div className="col-sm-4">
                    <SimpleImageUpload ref="drawControl" />
                    <div className="m-b-10 mt-3">
                        <button className="btn btn-primary m-r-10" onClick={() => {
                            this.props.tryPredictNumber((this.refs["drawControl"] as SimpleImageUpload).getImageData(), true);
                        }}>Predict Grpc</button>
                        <button className="btn btn-primary m-r-10" onClick={() => {
                            this.props.tryPredictNumber((this.refs["drawControl"] as SimpleImageUpload).getImageData(), false);
                        }}>Predict Rest</button>
                        <button className="btn btn-secondary" onClick={() => {
                            (this.refs["drawControl"] as SimpleImageUpload).clear();
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
                                        <p><b>It is: {this.props.predictedValue}</b></p>
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
    (state: ApplicationState) => state.catsAndDogsPredict, 
    CatsAndDogsPredictStore.actionCreators                 
)(CatsAndDogsPredictComponent as any);