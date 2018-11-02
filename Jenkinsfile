pipeline {
  agent any
  stages {
    stage('build') {
      steps {
        bat '%MSBUILD15% gitrevsubst.sln /p:Configuration=Release'
      }
    }
  }
}