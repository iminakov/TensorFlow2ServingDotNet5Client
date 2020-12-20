import * as React from 'react';
import CanvasDraw from "react-canvas-draw";

export class DrawComponent2 extends React.Component<any, any> {

    constructor(props: any) {
        super(props);
        this.clear = this.clear.bind(this);
    }

    public getImageData() {
        let image = (this.refs["paint_region"] as any).canvas.drawing.toDataURL("image/png");
        return image.replace('data:image/png;base64,', '');
    }

    public clear() {
        (this.refs["paint_region"] as any).clear();
    }

    public render() {
        let clearValue = this.state != null ? this.state.clearing : true;

        if (!clearValue) {
            return (
                <div className="paint_region_container">
                    <div className="paint_region_clear">
                        <br />
                    </div>
                    <div className="paint_region_hack">
                        <br />
                    </div>
                </div>);
        }

        return (
            <div className="paint_region_container">
                <CanvasDraw
                    ref="paint_region"
                    brushColor='#ffffff'
                    backgroundColor="#000000"
                    hideGrid={true}
                    brushRadius={10}
                    lazyRadius={2}
                    canvasWidth={280}
                    canvasHeight={280}
                />
            </div>);
    }
}