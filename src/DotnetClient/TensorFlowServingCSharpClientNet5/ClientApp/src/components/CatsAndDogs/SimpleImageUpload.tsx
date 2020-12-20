import * as React from 'react';

export default class SimpleImageUpload extends React.Component<any, any> {
  
  clear() {
    this.setState({
      file: null,
      fileData: ''
    })
  }

  getImageData(): any {
    return this.state.fileData.replace('data:image/png;base64,', '').replace('data:image/jpg;base64,', '').replace('data:image/jpeg;base64,', '');
  }

  constructor(props: any) {
    super(props)
    this.state = {
      file: null,
      fileData: ''
    }
    this.handleChange = this.handleChange.bind(this)
  }

  handleChange(event: any) {
    var reader = new FileReader();
    reader.readAsDataURL(event.target.files[0]);
    const _this = this;
    reader.onloadend = function () {
      _this.setState({
        fileData: reader.result
      });
    }

    this.setState({
      file: URL.createObjectURL(event.target.files[0])
    })
  }

  render() {
    return (
      <div className="paint_region_container">
        <input type="file" onChange={this.handleChange} />
        { (this.state.file) ? (<img style={{ width: '100%' }} alt="Loaded example" src={this.state.file} />) : null}
      </div>
    );
  }
}